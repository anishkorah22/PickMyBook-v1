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
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public Query(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }

    public async Task<IEnumerable<Book>> GetBooks()
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var bookService = scope.ServiceProvider.GetRequiredService<IBookService>();
        return await bookService.GetBooksAsync();
    }
    public async Task<IEnumerable<Book>> GetAllBooks()
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var bookService = scope.ServiceProvider.GetRequiredService<IBookService>();
        return await bookService.GetAllBooksAsync();
    }

    public async Task<IEnumerable<Borrowings>> GetBorrowings()
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var borrowingService = scope.ServiceProvider.GetRequiredService<IBorrowingService>();
        return await borrowingService.GetBorrowingsAsync();
    }

    public async Task<List<Borrowings>> GetBorrowingsByUserIdAsync(int userId)
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var borrowingService = scope.ServiceProvider.GetRequiredService<IBorrowingService>();
        return await borrowingService.GetBorrowingsByUserIdAsync(userId);
    }

    public async Task<IEnumerable<User>> GetAllUsers()
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var userService = scope.ServiceProvider.GetRequiredService<IUserService>();
        return await userService.GetAllUsersAsync();
    }

    public async Task<IEnumerable<User>> GetUsers()
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var userService = scope.ServiceProvider.GetRequiredService<IUserService>();
        return await userService.GetUsersAsync();
    }
    public User GetUserById(int id)
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<LibraryContext>();
        return context.Users.FirstOrDefault(u => u.UserId == id);
    }

    public async Task<DashboardCountsDTO> GetDashboardCountsAsync()
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var bookService = scope.ServiceProvider.GetRequiredService<IBookService>();
        var userService = scope.ServiceProvider.GetRequiredService<IUserService>();
        var borrowingService = scope.ServiceProvider.GetRequiredService<IBorrowingService>();

        var totalBooks = await bookService.GetTotalBooksCountAsync();
        var totalActiveUsers = await userService.GetTotalActiveUsersCountAsync();
        var totalCurrentBorrowTransactions = await borrowingService.GetTotalBorrowingsCountAsync();

        return new DashboardCountsDTO
        {
            TotalBooks = totalBooks,
            TotalActiveUsers = totalActiveUsers,
            TotalCurrentBorrowTransactions = totalCurrentBorrowTransactions
        };
    }

    public async Task<IEnumerable<RequestDTO>> GetAllRequestsAsync()
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var requestService = scope.ServiceProvider.GetRequiredService<IRequestService>();
        return await requestService.GetAllRequestsAsync();

    }

    public async Task<List<UserBooksReadInfoDTO>> GetBooksReadByUser(int userId)
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var borrowingService = scope.ServiceProvider.GetRequiredService<IBorrowingService>();
        return await borrowingService.GetBooksReadByUserAsync(userId);
    }

    public async Task<IEnumerable<UserRequestsDTO>> GetUserRequests (int userId)
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var requestService = scope.ServiceProvider.GetRequiredService<IRequestService>();
        return await requestService.GetRequestsByUserAsync(userId);

    }
}