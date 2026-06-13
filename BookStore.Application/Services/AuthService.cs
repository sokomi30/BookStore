using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using BookStore.Application.DTOs;
using BookStore.Domain.Models;
using BookStore.Infrastructure.Data;

namespace BookStore.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthService(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<AuthResponseDto> RegisterAsync(RegisterDto dto)
        {
            if (await _context.Users.AnyAsync(u => u.Username == dto.Username))
                throw new InvalidOperationException("Username already exists");

            var user = new User
            {
                Username = dto.Username,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                Role = "User"
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var response = GenerateTokens(user);
            await _context.SaveChangesAsync();
            return response;
        }

        public async Task<AuthResponseDto?> LoginAsync(LoginDto dto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == dto.Username);
            if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
                return null;

            var response = GenerateTokens(user);
            await _context.SaveChangesAsync();
            return response;
        }

        public async Task<AuthResponseDto?> RefreshTokenAsync(string refreshToken)
        {
            var user = await _context.Users.FirstOrDefaultAsync(
                u => u.RefreshToken == refreshToken && u.RefreshTokenExpiry > DateTime.UtcNow);

            if (user == null) return null;

            var response = GenerateTokens(user);
            await _context.SaveChangesAsync();
            return response;
        }

        private string GenerateRefreshToken()
        {
            return Convert.ToBase64String(Guid.NewGuid().ToByteArray());
        }

        private void SetRefreshToken(User user)
        {
            user.RefreshToken = GenerateRefreshToken();
            user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(7);
        }

        private AuthResponseDto GenerateTokens(User user)
        {
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(15),
                signingCredentials: credentials
            );

            SetRefreshToken(user);

            return new AuthResponseDto
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                RefreshToken = user.RefreshToken,
                Username = user.Username,
                Role = user.Role
            };
        }
    }
}