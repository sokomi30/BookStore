using AutoMapper;
using BookStore.Application.DTOs;
using BookStore.Application.Mappings;
using BookStore.Application.Services;
using BookStore.Domain.Models;
using BookStore.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace BookStore.Tests.Unit.Services
{
    public class BookServiceTests : IDisposable
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly BookService _bookService;

        public BookServiceTests()
        {
            // InMemory database for testing
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            _context = new AppDbContext(options);

            // Real AutoMapper with our profiles
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<BookProfile>();
                cfg.AddProfile<AuthorProfile>();
            }, NullLoggerFactory.Instance);
            _mapper = config.CreateMapper();

            var cacheMock = new Mock<ICacheService>();
            _bookService = new BookService(_context, _mapper, cacheMock.Object);

            SeedData();
        }

        private void SeedData()
        {
            _context.Authors.Add(new Author
            {
                Id = 1,
                FullName = "Test Author",
                BirthDate = new DateTime(1980, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            });

            _context.Books.AddRange(
                new Book { Id = 1, ISBN = "1111111111", Title = "Book One", Price = 100, AuthorId = 1 },
                new Book { Id = 2, ISBN = "2222222222", Title = "Book Two", Price = 200, AuthorId = 1 },
                new Book { Id = 3, ISBN = "3333333333", Title = "Another", Price = 300, AuthorId = 1 }
            );

            _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [Fact]
        public async Task GetAllAsync_ReturnsAllBooks()
        {
            // Act
            var result = await _bookService.GetAllAsync();

            // Assert
            Assert.Equal(3, result.Count);
        }

        [Fact]
        public async Task GetByIdAsync_ExistingId_ReturnsBook()
        {
            // Act
            var result = await _bookService.GetByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Book One", result.Title);
            Assert.Equal("Test Author", result.AuthorFullName);
        }

        [Fact]
        public async Task GetByIdAsync_NonExistingId_ReturnsNull()
        {
            // Act
            var result = await _bookService.GetByIdAsync(999);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task SearchAsync_ByTitle_ReturnsMatchingBooks()
        {
            // Act
            var result = await _bookService.SearchAsync("Book", null);

            // Assert
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public async Task SearchAsync_ByAuthor_ReturnsMatchingBooks()
        {
            // Act
            var result = await _bookService.SearchAsync(null, "Test");

            // Assert
            Assert.Equal(3, result.Count);
        }

        [Fact]
        public async Task SearchAsync_NoMatches_ReturnsEmptyList()
        {
            // Act
            var result = await _bookService.SearchAsync("Nonexistent", null);

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public async Task CreateAsync_ValidDto_CreatesBook()
        {
            // Arrange
            var dto = new CreateBookDto
            {
                ISBN = "9999999999",
                Title = "New Book",
                Price = 500,
                AuthorId = 1
            };

            // Act
            var result = await _bookService.CreateAsync(dto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("New Book", result.Title);
            Assert.Equal(4, await _context.Books.CountAsync());
        }

        [Fact]
        public async Task UpdateAsync_ExistingBook_UpdatesBook()
        {
            // Arrange
            var dto = new UpdateBookDto
            {
                ISBN = "1111111111",
                Title = "Updated Book",
                Price = 150
            };

            // Act
            var result = await _bookService.UpdateAsync(1, dto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Updated Book", result.Title);
        }

        [Fact]
        public async Task UpdateAsync_NonExistingBook_ReturnsNull()
        {
            // Act
            var result = await _bookService.UpdateAsync(999, new UpdateBookDto());

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task DeleteAsync_ExistingBook_ReturnsTrue()
        {
            // Act
            var result = await _bookService.DeleteAsync(1);

            // Assert
            Assert.True(result);
            Assert.Equal(2, await _context.Books.CountAsync());
        }

        [Fact]
        public async Task DeleteAsync_NonExistingBook_ReturnsFalse()
        {
            // Act
            var result = await _bookService.DeleteAsync(999);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task GetPaginatedAsync_ReturnsCorrectPage()
        {
            // Act
            var result = await _bookService.GetPaginatedAsync(1, 2);

            // Assert
            Assert.Equal(2, result.Items.Count);
            Assert.Equal(3, result.TotalCount);
            Assert.Equal(2, result.TotalPages);
            Assert.True(result.HasNextPage);
        }
    }
}