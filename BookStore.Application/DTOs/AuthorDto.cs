namespace BookStore.Application.DTOs
{
    public class AuthorDto
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public DateTime BirthDate { get; set; }
    }
}