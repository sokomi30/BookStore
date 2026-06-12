using BookStore.Application.DTOs;

namespace BookStore.Application.Services
{
    public interface IBookService
    {
        Task<List<BookDto>> GetAllAsync();
        Task<BookDto?> GetByIdAsync(int id);
        Task<BookDto> CreateAsync(CreateBookDto dto);
        Task<BookDto?> UpdateAsync(int id, CreateBookDto dto);
        Task<bool> DeleteAsync(int id);
    }
}