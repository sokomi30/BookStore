namespace BookStore.Domain.Models
{
    public class Book
    {
        public int Id { get; set; }
        public string ISBN { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int AuthorId { get; set; }
        public Author Author { get; set; } = null!;
        public string? CoverImagePath { get; set; }
    }
}