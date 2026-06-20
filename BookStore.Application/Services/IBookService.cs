using BookStore.Application.DTOs;

namespace BookStore.Application.Services
{
    public interface IBookService
    {
        Task<List<BookDto>> GetAllAsync();
        Task<BookDto?> GetByIdAsync(int id);
        Task<BookDto> CreateAsync(CreateBookDto dto);
        Task<BookDto?> UpdateAsync(int id, UpdateBookDto dto);
        Task<bool> DeleteAsync(int id); 
        Task<List<BookDto>> SearchAsync(string? title, string? author);
        Task<PaginatedResult<BookDto>> GetPaginatedAsync(int page, int pageSize);
    }
}