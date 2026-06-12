using BookStore.Domain.Models;

namespace BookStore.Infrastructure.Data
{
    public class BookStoreSeeder : IDataSeeder
    {
        private readonly int _bookCount;

        public BookStoreSeeder(int bookCount = 100)
        {
            _bookCount = bookCount;
        }

        public async Task SeedAsync(AppDbContext context)
        {
            if (context.Authors.Any()) return; // Уже есть данные

            var authors = GetAuthors();
            context.Authors.AddRange(authors);
            await context.SaveChangesAsync();

            var books = GenerateBooks(authors, _bookCount);
            context.Books.AddRange(books);
            await context.SaveChangesAsync();
        }

        private static List<Author> GetAuthors()
        {
            return new List<Author>
            {
                new() { FullName = "Александр Пушкин", BirthDate = new DateTime(1799, 6, 6, 0, 0, 0, DateTimeKind.Utc) },
                new() { FullName = "Лев Толстой", BirthDate = new DateTime(1828, 9, 9, 0, 0, 0, DateTimeKind.Utc) },
                new() { FullName = "Фёдор Достоевский", BirthDate = new DateTime(1821, 11, 11, 0, 0, 0, DateTimeKind.Utc) },
                new() { FullName = "Антон Чехов", BirthDate = new DateTime(1860, 1, 29, 0, 0, 0, DateTimeKind.Utc) },
                new() { FullName = "Михаил Булгаков", BirthDate = new DateTime(1891, 5, 15, 0, 0, 0, DateTimeKind.Utc) },
                new() { FullName = "Джордж Оруэлл", BirthDate = new DateTime(1903, 6, 25, 0, 0, 0, DateTimeKind.Utc) },
                new() { FullName = "Джон Толкин", BirthDate = new DateTime(1892, 1, 3, 0, 0, 0, DateTimeKind.Utc) },
                new() { FullName = "Джоан Роулинг", BirthDate = new DateTime(1965, 7, 31, 0, 0, 0, DateTimeKind.Utc) },
                new() { FullName = "Агата Кристи", BirthDate = new DateTime(1890, 9, 15, 0, 0, 0, DateTimeKind.Utc) },
                new() { FullName = "Иван Тургенев", BirthDate = new DateTime(1818, 11, 9, 0, 0, 0, DateTimeKind.Utc) },
                new() { FullName = "Николай Гоголь", BirthDate = new DateTime(1809, 4, 1, 0, 0, 0, DateTimeKind.Utc) },
                new() { FullName = "Михаил Лермонтов", BirthDate = new DateTime(1814, 10, 15, 0, 0, 0, DateTimeKind.Utc) },
                new() { FullName = "Эрнест Хемингуэй", BirthDate = new DateTime(1899, 7, 21, 0, 0, 0, DateTimeKind.Utc) },
                new() { FullName = "Рэй Брэдбери", BirthDate = new DateTime(1920, 8, 22, 0, 0, 0, DateTimeKind.Utc) },
                new() { FullName = "Габриэль Гарсиа Маркес", BirthDate = new DateTime(1927, 3, 6, 0, 0, 0, DateTimeKind.Utc) },
                new() { FullName = "Фрэнсис Скотт Фицджеральд", BirthDate = new DateTime(1896, 9, 24, 0, 0, 0, DateTimeKind.Utc) },
                new() { FullName = "Артур Конан Дойл", BirthDate = new DateTime(1859, 5, 22, 0, 0, 0, DateTimeKind.Utc) },
                new() { FullName = "Марк Твен", BirthDate = new DateTime(1835, 11, 30, 0, 0, 0, DateTimeKind.Utc) },
                new() { FullName = "Оскар Уайльд", BirthDate = new DateTime(1854, 10, 16, 0, 0, 0, DateTimeKind.Utc) },
                new() { FullName = "Чарльз Диккенс", BirthDate = new DateTime(1812, 2, 7, 0, 0, 0, DateTimeKind.Utc) }
            };
        }

        private static List<Book> GenerateBooks(List<Author> authors, int count)
        {
            var titles = new[]
            {
                "Война и мир", "Преступление и наказание", "Мастер и Маргарита", "Тихий Дон",
                "Евгений Онегин", "Герой нашего времени", "Мёртвые души", "Отцы и дети",
                "Братья Карамазовы", "Анна Каренина", "1984", "Скотный двор",
                "Хоббит", "Властелин колец", "Гарри Поттер", "Убийство в Восточном экспрессе",
                "Десять негритят", "451 градус по Фаренгейту", "Сто лет одиночества",
                "Великий Гэтсби", "Старик и море", "Портрет Дориана Грея",
                "Приключения Шерлока Холмса", "Приключения Тома Сойера", "Оливер Твист",
                "Вишнёвый сад", "Чайка", "Дядя Ваня", "Собачье сердце", "Белая гвардия",
                "Дубровский", "Капитанская дочка", "Муму", "Ревизор", "Тарас Бульба",
                "Идиот", "Бесы", "Игрок", "Воскресение", "Смерть Ивана Ильича",
                "Дворянское гнездо", "Рудин", "Накануне", "Обломов", "Гроза",
                "Шинель", "Нос", "Мцыри", "Демон", "Маскарад"
            };

            var random = new Random(42); // Фиксированный seed для воспроизводимости
            var books = new List<Book>();

            for (int i = 0; i < count; i++)
            {
                var titleIndex = i < titles.Length ? i : random.Next(titles.Length);
                var title = i < titles.Length ? titles[i] : $"{titles[titleIndex]} (часть {i / titles.Length + 1})";

                books.Add(new Book
                {
                    ISBN = $"978-5-17-{100000 + i:D6}",
                    Title = title,
                    Price = random.Next(200, 1500),
                    AuthorId = authors[random.Next(authors.Count)].Id
                });
            }

            return books;
        }
    }
}