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
            var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();

            context.Database.Migrate();
            await seeder.SeedAsync(context);

            if (app.Environment.IsDevelopment())
            {
                var password = configuration["AdminSeed:Password"] ?? "admin123";
                var passwordHash = BCrypt.Net.BCrypt.HashPassword(password);
                await AdminSeeder.SeedAdminAsync(context, configuration, passwordHash);
            }

            return app;
        }
    }
}