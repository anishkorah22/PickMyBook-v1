using System.ComponentModel.DataAnnotations.Schema;

namespace Experion.PickMyBook.Infrastructure.Models
{
    public class Borrowings
    {
        public int BorrowingsId { get; set; }
        public int UserId { get; set; }
        [ForeignKey("User")]
        public User User { get; set; }
        public int BookId { get; set; }
        [ForeignKey("Book")]
        public Book Book { get; set; }
        public DateTime BorrowDate { get; set; }
        public DateTime? ReturnDate { get; set; } 
        public string Status { get; set; } 
        public decimal? FineAmt { get; set; } 
    }
}
