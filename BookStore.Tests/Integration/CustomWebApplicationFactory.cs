using BookStore.Infrastructure.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;

namespace BookStore.Tests.Integration
{
    public class CustomWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram> where TProgram : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseSetting("ASPNETCORE_ENVIRONMENT", "Test");

            builder.ConfigureServices(services =>
            {
                // Удаляем PostgreSQL
                var dbContextDescriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(AppDbContext));
                if (dbContextDescriptor != null)
                    services.Remove(dbContextDescriptor);

                var dbContextOptionsDescriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(DbContextOptions<AppDbContext>));
                if (dbContextOptionsDescriptor != null)
                    services.Remove(dbContextOptionsDescriptor);

                var dbContextOptionsFactoryDescriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(IDbContextOptionsConfiguration<AppDbContext>));
                if (dbContextOptionsFactoryDescriptor != null)
                    services.Remove(dbContextOptionsFactoryDescriptor);

                services.AddDbContext<AppDbContext>(options =>
                    options.UseInMemoryDatabase("TestDb"));

                // Удаляем Redis и заменяем на MemoryCache
                var redisDescriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(IDistributedCache));
                if (redisDescriptor != null)
                    services.Remove(redisDescriptor);

                services.AddDistributedMemoryCache();

                // Seed
                var sp = services.BuildServiceProvider();
                using var scope = sp.CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                context.Database.EnsureCreated();

                if (!context.Authors.Any())
                {
                    context.Authors.Add(new Domain.Models.Author
                    {
                        Id = 1,
                        FullName = "Test Author",
                        BirthDate = new DateTime(1980, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                    });
                    context.Books.Add(new Domain.Models.Book
                    {
                        Id = 1,
                        ISBN = "1234567890",
                        Title = "Test Book",
                        Price = 100,
                        AuthorId = 1
                    });
                    context.SaveChanges();
                }
            });
        }
    }
}