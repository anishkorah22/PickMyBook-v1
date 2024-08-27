﻿using Experion.PickMyBook.Infrastructure.Models;
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

    public Query(IBookService bookService, IUserService userService, IBorrowingService borrowingService)
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

}
