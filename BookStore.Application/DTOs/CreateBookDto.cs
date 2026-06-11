namespace BookStore.Application.DTOs
{
    public class CreateBookDto
    {
        public string ISBN { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int AuthorId { get; set; }
    }
}
