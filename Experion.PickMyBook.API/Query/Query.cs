using Experion.PickMyBook.Infrastructure.Models;
using Experion.PickMyBook.Data;
using Experion.PickMyBook.Business.Services;
using Experion.PickMyBook.Business.Service;
using System.Linq;
using System.Threading.Tasks;
using Experion.PickMyBook.Infrastructure;

public class Query
{
    private readonly BookService _bookService;
    private readonly UserService _userService;
    private readonly BorrowingService _borrowingService;

    public Query(BookService bookService, UserService userService, BorrowingService borrowingService)
    {
        _bookService = bookService;
        _userService = userService;
        _borrowingService = borrowingService;
    }

    public IQueryable<Book> GetBooks([Service] LibraryContext context) => context.Books;

    public IQueryable<Borrowings> GetBorrowings([Service] LibraryContext context) => context.Borrowings;

    public async Task<IEnumerable<User>> GetAllUsers([Service] UserService userService)
    {
        return await userService.GetAllUsersAsync();
    }

    public User GetUserById(int id, [Service] LibraryContext context)
    {
        return context.Users.FirstOrDefault(u => u.UserId == id);
    }

    /*public async Task<int> GetBookCountAsync()
    {
        return await _bookService.GetTotalBooksAsync();
    }

    public async Task<int> GetUserCountAsync()
    {
        return await _userService.GetTotalActiveUsersAsync();
    }

    public async Task<int> GetBorrowingCountAsync()
    {
        return await _borrowingService.GetTotalCurrentBorrowTransactionsAsync();
    }*/
}
