
using Experion.PickMyBook.Infrastructure.Models;
using Experion.PickMyBook.Data;


public class Query
{
    public IQueryable<Book> GetBooks([Service] LibraryContext context) => context.Books;

    public IQueryable<User> GetUsers([Service] LibraryContext context) => context.Users;

    public IQueryable<Borrowings> GetBorrowings([Service] LibraryContext context) => context.Borrowings;

    public User GetUserByUsername([Service] LibraryContext context, string userName)
    {
        return context.Users.FirstOrDefault(u => u.UserName == userName);
    }
}
