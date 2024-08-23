using HotChocolate;
using Experion.PickMyBook.Infrastructure.Models;
using Experion.PickMyBook.Data;
using System.Threading.Tasks;
using HotChocolate.Authorization;
using Experion.PickMyBook.Infrastructure;

public class Mutation
{
    [Authorize]
    public async Task<Book> AddBook([Service] LibraryContext context, Book book)
    {
        context.Books.Add(book);
        await context.SaveChangesAsync();
        return book;
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
