namespace BookStore.Application.DTOs
{
    public class CreateAuthorDto
    {
        public string FullName { get; set; } = string.Empty;
        public DateTime BirthDate { get; set; }
    }
}