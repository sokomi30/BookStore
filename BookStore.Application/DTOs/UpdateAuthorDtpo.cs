namespace BookStore.Application.DTOs
{
    public class UpdateAuthorDto
    {
        public string FullName { get; set; } = string.Empty;
        public DateTime BirthDate { get; set; }
    }
}