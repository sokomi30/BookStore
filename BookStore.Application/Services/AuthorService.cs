using AutoMapper;
using Microsoft.EntityFrameworkCore;
using BookStore.Application.DTOs;
using BookStore.Application.Services;
using BookStore.Domain.Models;
using BookStore.Infrastructure.Data;

namespace BookStore.Application.Services
{
    public class AuthorService : IAuthorService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public AuthorService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<AuthorDto>> GetAllAsync()
        {
            var authors = await _context.Authors.ToListAsync();
            return _mapper.Map<List<AuthorDto>>(authors);
        }

        public async Task<AuthorDto?> GetByIdAsync(int id)
        {
            var author = await _context.Authors.FindAsync(id);
            return author == null ? null : _mapper.Map<AuthorDto>(author);
        }

        public async Task<AuthorDto> CreateAsync(CreateAuthorDto dto)
        {
            var author = _mapper.Map<Author>(dto);
            author.BirthDate = DateTime.SpecifyKind(author.BirthDate, DateTimeKind.Utc);

            _context.Authors.Add(author);
            await _context.SaveChangesAsync();

            return _mapper.Map<AuthorDto>(author);
        }

        public async Task<AuthorDto?> UpdateAsync(int id, UpdateAuthorDto dto)
        {
            var author = await _context.Authors.FindAsync(id);
            if (author == null) return null;

            _mapper.Map(dto, author);
            author.BirthDate = DateTime.SpecifyKind(author.BirthDate, DateTimeKind.Utc);

            await _context.SaveChangesAsync();
            return _mapper.Map<AuthorDto>(author);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var author = await _context.Authors.FindAsync(id);
            if (author == null) return false;

            _context.Authors.Remove(author);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}