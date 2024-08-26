using HotChocolate;
using Experion.PickMyBook.Infrastructure.Models;
using Experion.PickMyBook.Data;
using System.Threading.Tasks;
using HotChocolate.Authorization;
using Experion.PickMyBook.Infrastructure;
using Experion.PickMyBook.Infrastructure.Models.DTO;

public class Mutation
{
    /*    [Authorize]
    */
    public async Task<Book> AddBook([Service] LibraryContext context, AddBooksDTO dto)
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

        context.Books.Add(book);
        await context.SaveChangesAsync();
        return book;
    }
    public async Task<Book> UpdateBook([Service] LibraryContext context, Book book)
    {
        // Find the existing book by ID
        var existingBook = await context.Books.FindAsync(book.BookId);

        if (existingBook == null)
        {
            throw new KeyNotFoundException("Book not found.");
        }

        // Update properties of the existing book with non-null values from the book object
        existingBook.Title = !string.IsNullOrEmpty(book.Title) ? book.Title : existingBook.Title;
        existingBook.Author = !string.IsNullOrEmpty(book.Author) ? book.Author : existingBook.Author;
        existingBook.ISBN = !string.IsNullOrEmpty(book.ISBN) ? book.ISBN : existingBook.ISBN;
        existingBook.Publisher = !string.IsNullOrEmpty(book.Publisher) ? book.Publisher : existingBook.Publisher;
        existingBook.AvailableCopies = book.AvailableCopies != 0 || existingBook.AvailableCopies == 0 ? book.AvailableCopies : existingBook.AvailableCopies;
        existingBook.PublishedYear = book.PublishedYear != 0 || existingBook.PublishedYear == 0 ? book.PublishedYear : existingBook.PublishedYear;
        existingBook.Genre = !string.IsNullOrEmpty(book.Genre) ? book.Genre : existingBook.Genre;

        // Only update UpdatedAt
        existingBook.UpdatedAt = DateTime.UtcNow;

        // Mark the entity as modified
        context.Books.Update(existingBook);
        await context.SaveChangesAsync();

        return existingBook;
    }


    [Authorize(Roles = new[] { "Admin" })]
    public async Task<User> AddUser([Service] LibraryContext context, User user)
    {
        context.Users.Add(user);
        await context.SaveChangesAsync();
        return user;
    }

    public async Task<Borrowings> AddBorrowing([Service] LibraryContext context, Borrowings borrowing)
    {
        context.Borrowings.Add(borrowing);
        await context.SaveChangesAsync();
        return borrowing;
    }

    public async Task<User> CreateUser([Service] LibraryContext context, string userName, IEnumerable<string> roles)
    {
        var newUser = new User
        {
            UserName = userName,
            Roles = roles,
            IsDeleted = false,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        context.Users.Add(newUser);
        await context.SaveChangesAsync();
        return newUser;
    }
}
