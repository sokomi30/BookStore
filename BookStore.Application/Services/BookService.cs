using AutoMapper;
using Microsoft.EntityFrameworkCore;
using BookStore.Application.DTOs;
using BookStore.Application.Services;
using BookStore.Domain.Models;
using BookStore.Infrastructure.Data;

namespace BookStore.Application.Services
{
    public class BookService : IBookService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly ICacheService _cache;
        private static readonly TimeSpan CacheDuration = TimeSpan.FromMinutes(5);

        public BookService(AppDbContext context, IMapper mapper, ICacheService cache)
        {
            _context = context;
            _mapper = mapper;
            _cache = cache;
        }

        public async Task<List<BookDto>> GetAllAsync()
        {
            var cached = await _cache.GetAsync<List<BookDto>>("books:all");
            if (cached != null) return cached;

            var books = await _context.Books.Include(b => b.Author).ToListAsync();
            var result = _mapper.Map<List<BookDto>>(books);

            await _cache.SetAsync("books:all", result, CacheDuration);
            return result;
        }

        public async Task<BookDto?> GetByIdAsync(int id)
        {
            var book = await _context.Books
                .Include(b => b.Author)
                .FirstOrDefaultAsync(b => b.Id == id);
            return book == null ? null : _mapper.Map<BookDto>(book);
        }

        public async Task<List<BookDto>> SearchAsync(string? title, string? author)
        {
            var query = _context.Books.Include(b => b.Author).AsQueryable();

            if (!string.IsNullOrWhiteSpace(title))
                query = query.Where(b => b.Title.Contains(title));

            if (!string.IsNullOrWhiteSpace(author))
                query = query.Where(b => b.Author.FullName.Contains(author));

            var books = await query.ToListAsync();
            return _mapper.Map<List<BookDto>>(books);
        }

        public async Task<PaginatedResult<BookDto>> GetPaginatedAsync(int page, int pageSize)
        {
            pageSize = Math.Clamp(pageSize, 1, 50);

            var totalCount = await _context.Books.CountAsync();
            var books = await _context.Books
                .Include(b => b.Author)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PaginatedResult<BookDto>
            {
                Items = _mapper.Map<List<BookDto>>(books),
                Page = page,
                PageSize = pageSize,
                TotalCount = totalCount
            };
        }

        public async Task<BookDto> CreateAsync(CreateBookDto dto)
        {
            var book = _mapper.Map<Book>(dto);
            _context.Books.Add(book);
            await _context.SaveChangesAsync();

            await _cache.RemoveAsync("books:all");
            return _mapper.Map<BookDto>(book);
        }

        public async Task<BookDto?> UpdateAsync(int id, CreateBookDto dto)
        {
            var book = await _context.Books.FindAsync(id);
            if (book == null) return null;

            _mapper.Map(dto, book);
            await _context.SaveChangesAsync();

            await _cache.RemoveAsync("books:all");
            return _mapper.Map<BookDto>(book);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book == null) return false;

            _context.Books.Remove(book);
            await _context.SaveChangesAsync();

            await _cache.RemoveAsync("books:all");
            return true;
        }
    }
}