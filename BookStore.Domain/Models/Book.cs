namespace BookStore.Domain.Models
{
    public class Book
    {
        public int Id { get; set; }
        public string ISBN { get; set; }
        public string Title { get; set; }
        public decimal Price { get; set; }

        // Внешний ключ
        public int AuthorId { get; set; }
        // Навигационное свойство
        public Author Author { get; set; }
    }
}
