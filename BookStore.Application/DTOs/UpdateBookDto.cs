namespace BookStore.Application.DTOs
{
    public class UpdateBookDto
    {
        public string? ISBN { get; set; }
        public string? Title { get; set; }
        public decimal? Price { get; set; }
        // ❌ Не включаем AuthorId в Update! Автора менять нельзя через этот DTO
        // Если нужно перенести книгу к другому автору, нужен отдельный endpoint
    }
}
