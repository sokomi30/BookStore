using BookStore.Application.DTOs;

namespace BookStore.Application.Services
{
    public interface IAuthorService
    {
        Task<List<AuthorDto>> GetAllAsync();
        Task<AuthorDto?> GetByIdAsync(int id);
        Task<PaginatedResult<AuthorDto>> GetPaginatedAsync(int page, int pageSize);
        Task<AuthorDto> CreateAsync(CreateAuthorDto dto);
        Task<AuthorDto?> UpdateAsync(int id, UpdateAuthorDto dto);
        Task<bool> DeleteAsync(int id);
    }
}