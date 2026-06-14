namespace BookStore.Application.DTOs
{
    public class BookDto
    {
        public int Id { get; set; }
        public string ISBN { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int AuthorId { get; set; }
        public string AuthorFullName { get; set; } = string.Empty; // Достанем только имя, а не весь объект
    }
}
