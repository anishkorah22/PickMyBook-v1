using HotChocolate;
using Experion.PickMyBook.Infrastructure.Models;
using Experion.PickMyBook.Data;
using System.Threading.Tasks;

public class Mutation
{
    public async Task<Book> AddBook([Service] LibraryContext context, Book book)
    {
        context.Books.Add(book);
        await context.SaveChangesAsync();
        return book;
    }

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
}
