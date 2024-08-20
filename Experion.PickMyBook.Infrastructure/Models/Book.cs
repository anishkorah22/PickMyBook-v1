namespace Experion.PickMyBook.Infrastructure.Models
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string ISBN { get; set; }
        public string Publisher { get; set; }
        public int AvailableCopies { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int PublishedYear { get; set; }
        public string Genre { get; set; }
        public ICollection<Borrowings> Borrowings { get; set; } = new List<Borrowings>();

    }
}
