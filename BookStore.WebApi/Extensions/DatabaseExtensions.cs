using Microsoft.EntityFrameworkCore;
using BookStore.Infrastructure.Data;

namespace BookStore.WebApi.Extensions
{
    public static class DatabaseExtensions
    {
        public static IServiceCollection AddBookStoreDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("Default")));

            return services;
        }
    }
}