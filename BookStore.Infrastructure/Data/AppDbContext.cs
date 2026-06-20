using Microsoft.EntityFrameworkCore;
using BookStore.Domain.Models;

namespace BookStore.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Book> Books { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // ===== BOOKS =====
            modelBuilder.Entity<Book>(entity =>
            {
                entity
                    .HasOne(b => b.Author)
                    .WithMany(a => a.Books)
                    .HasForeignKey(b => b.AuthorId)
                    .OnDelete(DeleteBehavior.Restrict); // ❌ Нельзя удалить автора если у него есть книги

                // Constraints (ограничения на уровне БД)
                entity.Property(b => b.ISBN)
                    .HasMaxLength(13)
                    .IsRequired();

                entity.Property(b => b.Title)
                    .HasMaxLength(200)
                    .IsRequired();

                entity.Property(b => b.Price)
                    .HasPrecision(10, 2); // Максимум 99999999.99

                // Индексы (ускоряют поиск)
                entity.HasIndex(b => b.ISBN).IsUnique(); // ISBN должен быть уникален
                entity.HasIndex(b => b.Title); // Для LIKE запросов
                entity.HasIndex(b => b.AuthorId);
            });

            // ===== AUTHORS =====
            modelBuilder.Entity<Author>(entity =>
            {
                entity.Property(a => a.FullName)
                    .HasMaxLength(150)
                    .IsRequired();

                // Индекс для поиска
                entity.HasIndex(a => a.FullName);
            });

            // ===== USERS =====
            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(u => u.Username)
                    .HasMaxLength(50)
                    .IsRequired();

                entity.Property(u => u.PasswordHash)
                    .HasMaxLength(255)
                    .IsRequired();

                entity.Property(u => u.Role)
                    .HasMaxLength(20)
                    .IsRequired();

                // Username должен быть уникален
                entity.HasIndex(u => u.Username).IsUnique();
            });
        }
    }
}