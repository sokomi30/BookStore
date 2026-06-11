namespace BookStore.Domain.Models
{
    public class Author
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public DateTime BirthDate { get; set; }

        // Навигационное свойство для связи "один ко многим"
        public ICollection<Book> Books { get; set; } = new List<Book>();
    }
}
