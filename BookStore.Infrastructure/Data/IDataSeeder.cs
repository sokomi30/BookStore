namespace BookStore.Infrastructure.Data
{
    public interface IDataSeeder
    {
        Task SeedAsync(AppDbContext context);
    }
}