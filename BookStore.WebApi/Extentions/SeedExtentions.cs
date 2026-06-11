using BookStore.Infrastructure.Data;

namespace BookStore.WebApi.Extensions
{
    public static class SeedExtensions
    {
        public static WebApplication UseDatabaseSeeding(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            DbSeeder.Seed(context);
            return app;
        }
    }
}