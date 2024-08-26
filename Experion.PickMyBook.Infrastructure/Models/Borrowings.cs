using Experion.PickMyBook.Infrastructure.Models;
using System.ComponentModel.DataAnnotations.Schema;

public class Borrowings
{
    [ForeignKey("Book")]
    public int BookId { get; set; }
    public Book? Book { get; set; }

    [ForeignKey("User")]
    public int UserId { get; set; }
    public User? User { get; set; }

    public DateTime? BorrowDate { get; set; } = DateTime.Now;
    public DateTime? ReturnDate { get; set; } = DateTime.UtcNow.AddDays(14);
    public string Status { get; set; }
    public decimal? FineAmt { get; set; }
}
