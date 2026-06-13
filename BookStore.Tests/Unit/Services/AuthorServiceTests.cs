using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using BookStore.Application.DTOs;
using BookStore.Application.Mappings;
using BookStore.Application.Services;
using BookStore.Domain.Models;
using BookStore.Infrastructure.Data;

namespace BookStore.Tests.Unit.Services
{
    public class AuthorServiceTests : IDisposable
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly AuthorService _authorService;

        public AuthorServiceTests()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            _context = new AppDbContext(options);

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<AuthorProfile>();
            }, NullLoggerFactory.Instance);
            _mapper = config.CreateMapper();

            _authorService = new AuthorService(_context, _mapper);

            SeedData();
        }

        private void SeedData()
        {
            _context.Authors.AddRange(
                new Author { Id = 1, FullName = "Author One", BirthDate = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
                new Author { Id = 2, FullName = "Author Two", BirthDate = new DateTime(1980, 2, 2, 0, 0, 0, DateTimeKind.Utc) }
            );
            _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [Fact]
        public async Task GetAllAsync_ReturnsAllAuthors()
        {
            var result = await _authorService.GetAllAsync();
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public async Task GetByIdAsync_ExistingId_ReturnsAuthor()
        {
            var result = await _authorService.GetByIdAsync(1);
            Assert.NotNull(result);
            Assert.Equal("Author One", result.FullName);
        }

        [Fact]
        public async Task GetByIdAsync_NonExistingId_ReturnsNull()
        {
            var result = await _authorService.GetByIdAsync(999);
            Assert.Null(result);
        }

        [Fact]
        public async Task CreateAsync_ValidDto_CreatesAuthor()
        {
            var dto = new CreateAuthorDto
            {
                FullName = "New Author",
                BirthDate = new DateTime(1990, 5, 5, 0, 0, 0, DateTimeKind.Utc)
            };

            var result = await _authorService.CreateAsync(dto);

            Assert.NotNull(result);
            Assert.Equal("New Author", result.FullName);
            Assert.Equal(3, await _context.Authors.CountAsync());
        }

        [Fact]
        public async Task UpdateAsync_ExistingAuthor_UpdatesAuthor()
        {
            var dto = new UpdateAuthorDto
            {
                FullName = "Updated Author",
                BirthDate = new DateTime(1975, 3, 3, 0, 0, 0, DateTimeKind.Utc)
            };

            var result = await _authorService.UpdateAsync(1, dto);

            Assert.NotNull(result);
            Assert.Equal("Updated Author", result.FullName);
        }

        [Fact]
        public async Task UpdateAsync_NonExistingAuthor_ReturnsNull()
        {
            var result = await _authorService.UpdateAsync(999, new UpdateAuthorDto());
            Assert.Null(result);
        }

        [Fact]
        public async Task DeleteAsync_ExistingAuthor_ReturnsTrue()
        {
            var result = await _authorService.DeleteAsync(1);
            Assert.True(result);
            Assert.Equal(1, await _context.Authors.CountAsync());
        }

        [Fact]
        public async Task DeleteAsync_NonExistingAuthor_ReturnsFalse()
        {
            var result = await _authorService.DeleteAsync(999);
            Assert.False(result);
        }
    }
}