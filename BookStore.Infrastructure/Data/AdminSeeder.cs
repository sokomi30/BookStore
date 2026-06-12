using BookStore.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace BookStore.Infrastructure.Data
{
    public static class AdminSeeder
    {
        public static async Task SeedAdminAsync(AppDbContext context, IConfiguration configuration, string passwordHash)
        {
            if (await context.Users.AnyAsync(u => u.Role == "Admin"))
                return;

            var username = configuration["AdminSeed:Username"] ?? "admin";

            context.Users.Add(new User
            {
                Username = username,
                PasswordHash = passwordHash,
                Role = "Admin"
            });

            await context.SaveChangesAsync();
        }
    }
}