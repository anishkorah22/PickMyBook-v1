using Experion.PickMyBook.Infrastructure.Models;
using Experion.PickMyBook.Data;
using Experion.PickMyBook.Business.Services;
using Experion.PickMyBook.Business.Service;
using System.Linq;
using System.Threading.Tasks;
using Experion.PickMyBook.Infrastructure;
using Experion.PickMyBook.Infrastructure.Models.DTO;
using Experion.PickMyBook.Business.Service.IService;

public class Query
{
    private readonly IBookService _bookService;
    private readonly IUserService _userService;
    private readonly IBorrowingService _borrowingService;
    private readonly IRequestService _requestService;

    public Query(IBookService bookService, IUserService userService, IBorrowingService borrowingService, IRequestService requestService)
    {
        _bookService = bookService;
        _userService = userService;
        _borrowingService = borrowingService;
        _requestService = requestService;
    }

    public IQueryable<Book> GetBooks([Service] LibraryContext context) => context.Books;

    public IQueryable<Borrowings> GetBorrowings([Service] LibraryContext context) => context.Borrowings;
    
    public async Task<List<Borrowings>> GetBorrowingsByUserIdAsync(int userId)
    {
        return await _borrowingService.GetBorrowingsByUserIdAsync(userId);
    }

    public async Task<IEnumerable<User>> GetAllUsers([Service] UserService userService)
    {
        return await userService.GetAllUsersAsync();
    }

    public User GetUserById(int id, [Service] LibraryContext context)
    {
        return context.Users.FirstOrDefault(u => u.UserId == id);
    }

    public async Task<DashboardCountsDTO> GetDashboardCountsAsync()
    {
        var totalBooks = await _bookService.GetTotalBooksCountAsync();
        var totalActiveUsers = await _userService.GetTotalActiveUsersCountAsync();
        var totalCurrentBorrowTransactions = await _borrowingService.GetTotalBorrowingsCountAsync();

        return new DashboardCountsDTO
        {
            TotalBooks = totalBooks,
            TotalActiveUsers = totalActiveUsers,
            TotalCurrentBorrowTransactions = totalCurrentBorrowTransactions
        };
    }

    public async Task<IEnumerable<RequestDTO>> GetAllRequestsAsync()
    {
        return await _requestService.GetAllRequestsAsync();

    }

}
