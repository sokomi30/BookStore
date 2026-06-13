using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using BookStore.Application.DTOs;
using BookStore.Application.Services;
using BookStore.Domain.Models;
using BookStore.Infrastructure.Data;

namespace BookStore.Tests.Unit.Services
{
    public class AuthServiceTests : IDisposable
    {
        private readonly AppDbContext _context;
        private readonly AuthService _authService;

        public AuthServiceTests()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            _context = new AppDbContext(options);

            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string?>
                {
                    { "Jwt:Key", "ThisIsASuperSecretKeyForJWT_AtLeast32Characters!" },
                    { "Jwt:Issuer", "BookStoreApi" },
                    { "Jwt:Audience", "BookStoreClient" }
                })
                .Build();

            _authService = new AuthService(_context, configuration);
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [Fact]
        public async Task RegisterAsync_NewUser_ReturnsToken()
        {
            // Arrange
            var dto = new RegisterDto
            {
                Username = "newuser",
                Password = "password123"
            };

            // Act
            var result = await _authService.RegisterAsync(dto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("newuser", result.Username);
            Assert.Equal("User", result.Role);
            Assert.NotEmpty(result.Token);
        }

        [Fact]
        public async Task RegisterAsync_DuplicateUser_ThrowsException()
        {
            // Arrange
            _context.Users.Add(new User
            {
                Username = "existing",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("pass"),
                Role = "User"
            });
            await _context.SaveChangesAsync();

            var dto = new RegisterDto
            {
                Username = "existing",
                Password = "password123"
            };

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(
                () => _authService.RegisterAsync(dto));
        }

        [Fact]
        public async Task LoginAsync_ValidCredentials_ReturnsToken()
        {
            // Arrange
            _context.Users.Add(new User
            {
                Username = "testuser",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("password123"),
                Role = "User"
            });
            await _context.SaveChangesAsync();

            var dto = new LoginDto
            {
                Username = "testuser",
                Password = "password123"
            };

            // Act
            var result = await _authService.LoginAsync(dto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("testuser", result.Username);
            Assert.NotEmpty(result.Token);
        }

        [Fact]
        public async Task LoginAsync_WrongPassword_ReturnsNull()
        {
            // Arrange
            _context.Users.Add(new User
            {
                Username = "testuser",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("password123"),
                Role = "User"
            });
            await _context.SaveChangesAsync();

            var dto = new LoginDto
            {
                Username = "testuser",
                Password = "wrongpassword"
            };

            // Act
            var result = await _authService.LoginAsync(dto);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task LoginAsync_NonExistentUser_ReturnsNull()
        {
            // Arrange
            var dto = new LoginDto
            {
                Username = "nonexistent",
                Password = "password123"
            };

            // Act
            var result = await _authService.LoginAsync(dto);

            // Assert
            Assert.Null(result);
        }
    }
}