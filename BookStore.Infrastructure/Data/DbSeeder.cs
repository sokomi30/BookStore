using BookStore.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Infrastructure.Data
{
    public static class DbSeeder
    {
        public static void Seed(AppDbContext context)
        {
            context.Database.EnsureCreated();

            if (!context.Authors.Any())
            {
                context.Authors.AddRange(
                    new Author
                    {
                        FullName = "Александр Пушкин",
                        BirthDate = new DateTime(1799, 6, 6)
                    },
                    new Author
                    {
                        FullName = "Лев Толстой",
                        BirthDate = new DateTime(1828, 9, 9)
                    },
                    new Author
                    {
                        FullName = "Фёдор Достоевский",
                        BirthDate = new DateTime(1821, 11, 11)
                    }
                );
                context.SaveChanges();
            }
        }
    }
}