using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using BookStore.Application.DTOs;
using BookStore.Infrastructure.Data;
using BookStore.WebApi;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace BookStore.Tests.Integration.Controllers
{
    public class BooksControllerTests : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly CustomWebApplicationFactory<Program> _factory;
        private readonly HttpClient _client;

        public BooksControllerTests(CustomWebApplicationFactory<Program> factory)
        {
            _factory = factory;
            _client = factory.CreateClient();
        }

        private async Task<string> GetAdminTokenAsync()
        {
            // Регистрируем пользователя
            await _client.PostAsJsonAsync("/api/auth/register", new RegisterDto
            {
                Username = "admin_test",
                Password = "admin123"
            });

            // Меняем роль на Admin в тестовой БД
            var scope = _factory.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            var user = await context.Users.FirstAsync(u => u.Username == "admin_test");
            user.Role = "Admin";
            await context.SaveChangesAsync();

            // Логинимся
            var loginResponse = await _client.PostAsJsonAsync("/api/auth/login", new LoginDto
            {
                Username = "admin_test",
                Password = "admin123"
            });

            var authResponse = await loginResponse.Content.ReadFromJsonAsync<AuthResponseDto>();
            return authResponse!.Token;
        }

        [Fact]
        public async Task GetBooks_ReturnsOk()
        {
            var response = await _client.GetAsync("/api/books");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task GetBookById_ExistingId_ReturnsBook()
        {
            var response = await _client.GetAsync("/api/books/1");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var book = await response.Content.ReadFromJsonAsync<BookDto>();
            Assert.NotNull(book);
            Assert.Equal("Test Book", book.Title);
        }

        [Fact]
        public async Task GetBookById_NonExistingId_ReturnsNotFound()
        {
            var response = await _client.GetAsync("/api/books/999");
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task CreateBook_WithoutToken_ReturnsUnauthorized()
        {
            var response = await _client.PostAsJsonAsync("/api/books", new CreateBookDto());
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task CreateBook_WithAdminToken_ReturnsCreated()
        {
            var token = await GetAdminTokenAsync();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var dto = new CreateBookDto
            {
                ISBN = "9999999999",
                Title = "Integration Test Book",
                Price = 500,
                AuthorId = 1
            };

            var response = await _client.PostAsJsonAsync("/api/books", dto);
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }

        [Fact]
        public async Task SearchBooks_ReturnsMatchingResults()
        {
            var response = await _client.GetAsync("/api/books/search?title=Test");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var books = await response.Content.ReadFromJsonAsync<List<BookDto>>();
            Assert.NotEmpty(books!);
        }

        [Fact]
        public async Task GetPaginatedBooks_ReturnsCorrectPage()
        {
            var response = await _client.GetAsync("/api/books/paginated?page=1&pageSize=10");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}