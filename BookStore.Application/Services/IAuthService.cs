using BookStore.Application.DTOs;

namespace BookStore.Application.Services
{
    public interface IAuthService
    {
        Task<AuthResponseDto> RegisterAsync(RegisterDto dto);
        Task<AuthResponseDto?> LoginAsync(LoginDto dto);
        Task<AuthResponseDto?> RefreshTokenAsync(string refreshToken);
    }
}