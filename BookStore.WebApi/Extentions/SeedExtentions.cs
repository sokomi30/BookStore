using BookStore.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BookStore.WebApi.Extensions
{
    public static class SeedExtensions
    {
        public static async Task<WebApplication> UseDatabaseSeedingAsync(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            var seeder = scope.ServiceProvider.GetRequiredService<IDataSeeder>();

            context.Database.Migrate();
            await seeder.SeedAsync(context);

            return app;
        }
    }
}