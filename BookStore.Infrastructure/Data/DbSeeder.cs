using BookStore.Domain.Models;

namespace BookStore.Infrastructure.Data
{
    public static class DbSeeder
    {
        public static void Seed(AppDbContext context)
        {
            if (!context.Authors.Any())
            {
                context.Authors.AddRange(
                    new Author
                    {
                        FullName = "Александр Пушкин",
                        BirthDate = new DateTime(1799, 6, 6, 0, 0, 0, DateTimeKind.Utc)
                    },
                    new Author
                    {
                        FullName = "Лев Толстой",
                        BirthDate = new DateTime(1828, 9, 9, 0, 0, 0, DateTimeKind.Utc)
                    },
                    new Author
                    {
                        FullName = "Фёдор Достоевский",
                        BirthDate = new DateTime(1821, 11, 11, 0, 0, 0, DateTimeKind.Utc)
                    }
                );
                context.SaveChanges();
            }
        }
    }
}