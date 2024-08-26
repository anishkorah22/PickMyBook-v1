using Experion.PickMyBook.Infrastructure.Models.DTO;
using Experion.PickMyBook.Infrastructure;

public interface IBookService
{
    Task<Book> AddBookAsync(AddBooksDTO dto);
    Task<Book> UpdateBookAsync(Book book);
}

public class BookService : IBookService
{
    private readonly LibraryContext _context;

    public BookService(LibraryContext context)
    {
        _context = context;
    }

    public async Task<Book> AddBookAsync(AddBooksDTO dto)
    {
        var book = new Book
        {
            Title = dto.Title,
            Author = dto.Author,
            ISBN = dto.ISBN,
            Publisher = dto.Publisher,
            AvailableCopies = dto.AvailableCopies,
            PublishedYear = dto.PublishedYear,
            Genre = dto.Genre,
        };

        _context.Books.Add(book);
        await _context.SaveChangesAsync();
        return book;
    }

    public async Task<Book> UpdateBookAsync(Book book)
    {
        var existingBook = await _context.Books.FindAsync(book.BookId);

        if (existingBook == null)
        {
            throw new KeyNotFoundException("Book not found.");
        }

        existingBook.Title = !string.IsNullOrEmpty(book.Title) ? book.Title : existingBook.Title;
        existingBook.Author = !string.IsNullOrEmpty(book.Author) ? book.Author : existingBook.Author;
        existingBook.ISBN = !string.IsNullOrEmpty(book.ISBN) ? book.ISBN : existingBook.ISBN;
        existingBook.Publisher = !string.IsNullOrEmpty(book.Publisher) ? book.Publisher : existingBook.Publisher;
        existingBook.AvailableCopies = book.AvailableCopies != 0 || existingBook.AvailableCopies == 0 ? book.AvailableCopies : existingBook.AvailableCopies;
        existingBook.PublishedYear = book.PublishedYear != 0 || existingBook.PublishedYear == 0 ? book.PublishedYear : existingBook.PublishedYear;
        existingBook.Genre = !string.IsNullOrEmpty(book.Genre) ? book.Genre : existingBook.Genre;

        existingBook.UpdatedAt = DateTime.UtcNow;

        _context.Books.Update(existingBook);
        await _context.SaveChangesAsync();

        return existingBook;
    }
}
