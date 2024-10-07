using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

public class Book
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int BookId { get; set; }
    public string? Title { get; set; }
    public string? Author { get; set; }
    public string? ISBN { get; set; }
    public string? Publisher { get; set; }
    public int? AvailableCopies { get; set; }
    public bool? IsDeleted { get; set; } = false;
    public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; } = DateTime.UtcNow;
    public int? PublishedYear { get; set; }
    public string? Genre { get; set; }

    [MaxLength(3)]
    public string[]? ImageUrls { get; set; } = new string[3];
    public ICollection<Borrowings>? Borrowings { get; set; } = new List<Borrowings>();
}

