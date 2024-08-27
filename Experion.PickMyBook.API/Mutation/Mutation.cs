using Experion.PickMyBook.Business.Service;
using Experion.PickMyBook.Business.Services;
using Experion.PickMyBook.Infrastructure.Models.DTO;
using Experion.PickMyBook.Infrastructure.Models;
using Experion.PickMyBook.Infrastructure;
using HotChocolate.Authorization;
using Experion.PickMyBook.Business.Service.IService;

public class Mutation
{
    private readonly IBookService _bookService;
    private readonly IBorrowingService _borrowingService;

    public Mutation(IBookService bookService, IBorrowingService borrowingService)
    {
        _bookService = bookService;
        _borrowingService = borrowingService;
    }

    public Task<Book> AddBook([Service] LibraryContext context, AddBooksDTO dto) => _bookService.AddBookAsync(dto);

    public Task<Book> UpdateBook([Service] LibraryContext context, Book book) => _bookService.UpdateBookAsync(book);

    [Authorize(Roles = new[] { "Admin" })]
    public async Task<User> AddUser([Service] LibraryContext context, User user)
    {
        context.Users.Add(user);
        await context.SaveChangesAsync();
        return user;
    }
    public Task<Borrowings> BorrowBook([Service] LibraryContext context, int userId, int bookId) => _borrowingService.BorrowBookAsync(userId, bookId);

    public Task<Borrowings> UpdateBorrowing([Service] LibraryContext context, Borrowings borrowing) => _borrowingService.UpdateBorrowingAsync(borrowing);

    public Task<Borrowings> ReturnBook([Service] LibraryContext context, int userId, int bookId) => _borrowingService.ReturnBookAsync(userId, bookId);

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

    public async Task<User> UpdateUser([Service] LibraryContext context, User user)
    {
        var existingUser = await context.Users.FindAsync(user.UserId);

        if (existingUser == null)
        {
            throw new KeyNotFoundException("User not found.");
        }

        existingUser.UserName = user.UserName;
        existingUser.Roles = user.Roles;
        existingUser.IsDeleted = user.IsDeleted;
        existingUser.UpdatedAt = DateTime.UtcNow;

        context.Users.Update(existingUser);
        await context.SaveChangesAsync();

        return existingUser;
    }

}
