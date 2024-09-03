﻿using Experion.PickMyBook.Business.Service;
using Experion.PickMyBook.Business.Services;
using Experion.PickMyBook.Infrastructure.Models.DTO;
using Experion.PickMyBook.Infrastructure.Models;
using Experion.PickMyBook.Infrastructure;
using HotChocolate.Authorization;
using Experion.PickMyBook.Business.Service.IService;
using HotChocolate.Subscriptions;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

/*public class Mutation
{
    private readonly IBookService _bookService;
    private readonly IUserService _userService;
    private readonly IBorrowingService _borrowingService;
    private readonly IRequestService _requestService;
    private readonly ITopicEventSender _eventSender;

    public Mutation(IBookService bookService, IUserService userService, IBorrowingService borrowingService, IRequestService requestService, ITopicEventSender eventSender)
    {
        _bookService = bookService;
        _userService = userService;
        _borrowingService = borrowingService;
        _requestService = requestService;
        _eventSender = eventSender;
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
    public Task<Request> CreateBorrowRequestAsync(int bookId, int userId)
    {
        return _requestService.CreateBorrowRequestAsync(bookId, userId);
    }
    public async Task<Request> CreateReturnRequestAsync(int bookId, int userId)
    {
        return await _requestService.CreateReturnRequestAsync(bookId, userId);
    }
    public async Task<Request> ApproveRequestAsync(int requestId)
    {
        return await _requestService.ApproveRequestAsync(requestId);
    }

    public async Task<Request> DeclineRequestAsync(int requestId, string message)
    {
        return await _requestService.DeclineRequestAsync(requestId, message);
    }
    public async Task<User> CreateUser([Service] LibraryContext context, string userName, IEnumerable<string> roles)
    {
        var newUser = new User
        {
            UserName = userName,
            Roles = roles,
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

    public async Task<Book> UpdateBookStatusAsync(int bookId, bool isDeleted)
    {
        var updatedBook = await _bookService.UpdateBookStatusAsync(bookId, isDeleted);

        await _eventSender.SendAsync("OnBookStatusChanged", updatedBook);

        return updatedBook;
    }
    public async Task<User> UpdateUserStatusAsync(int userId, bool isDeleted)
    {
        var updatedUser = await _userService.UpdateUserStatusAsync(userId, isDeleted);

        await _eventSender.SendAsync("OnUserStatusChanged", updatedUser);

        return updatedUser;
    }

}*/

public class Mutation
{
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public Mutation(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }

    public async Task<Book> AddBook(AddBooksDTO dto)
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var context = scope.ServiceProvider.GetService<LibraryContext>();
        var bookService = scope.ServiceProvider.GetService<IBookService>();
        return await bookService.AddBookAsync(dto);
    }

    public async Task<Book> UpdateBook(Book book)
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var context = scope.ServiceProvider.GetService<LibraryContext>();
        var bookService = scope.ServiceProvider.GetService<IBookService>();
        return await bookService.UpdateBookAsync(book);
    }

    [Authorize(Roles = new[] { "Admin" })]
    public async Task<User> AddUser(User user)
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var context = scope.ServiceProvider.GetService<LibraryContext>();
        context.Users.Add(user);
        await context.SaveChangesAsync();
        return user;
    }

    public async Task<Borrowings> BorrowBook(int userId, int bookId)
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var borrowingService = scope.ServiceProvider.GetService<IBorrowingService>();
        return await borrowingService.BorrowBookAsync(userId, bookId);
    }

    public async Task<Borrowings> UpdateBorrowing(Borrowings borrowing)
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var borrowingService = scope.ServiceProvider.GetService<IBorrowingService>();
        return await borrowingService.UpdateBorrowingAsync(borrowing);
    }

    public async Task<Borrowings> ReturnBook(int userId, int bookId)
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var borrowingService = scope.ServiceProvider.GetService<IBorrowingService>();
        return await borrowingService.ReturnBookAsync(userId, bookId);
    }

    public async Task<Request> CreateBorrowRequestAsync(int bookId, int userId)
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var requestService = scope.ServiceProvider.GetService<IRequestService>();
        return await requestService.CreateBorrowRequestAsync(bookId, userId);
    }

    public async Task<Request> CreateReturnRequestAsync(int bookId, int userId)
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var requestService = scope.ServiceProvider.GetService<IRequestService>();
        return await requestService.CreateReturnRequestAsync(bookId, userId);
    }

    public async Task<Request> ApproveRequestAsync(int requestId)
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var requestService = scope.ServiceProvider.GetService<IRequestService>();
        return await requestService.ApproveRequestAsync(requestId);
    }

    public async Task<Request> DeclineRequestAsync(int requestId, string message)
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var requestService = scope.ServiceProvider.GetService<IRequestService>();
        return await requestService.DeclineRequestAsync(requestId, message);
    }

    public async Task<User> CreateUser(string userName, IEnumerable<string> roles)
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var context = scope.ServiceProvider.GetService<LibraryContext>();
        var userService = scope.ServiceProvider.GetService<IUserService>();
        var newUser = new User
        {
            UserName = userName,
            Roles = roles,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        context.Users.Add(newUser);
        await context.SaveChangesAsync();
        return newUser;
    }

    public async Task<User> UpdateUser(User user)
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var context = scope.ServiceProvider.GetService<LibraryContext>();
        var userService = scope.ServiceProvider.GetService<IUserService>();
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

    public async Task<Book> UpdateBookStatusAsync(int bookId, bool isDeleted)
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var bookService = scope.ServiceProvider.GetService<IBookService>();
        var eventSender = scope.ServiceProvider.GetService<ITopicEventSender>();
        var updatedBook = await bookService.UpdateBookStatusAsync(bookId, isDeleted);

        await eventSender.SendAsync("OnBookStatusChanged", updatedBook);

        return updatedBook;
    }
    public async Task<User> UpdateUserStatusAsync(int userId, bool isDeleted)
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var userService = scope.ServiceProvider.GetService<IUserService>();
        var eventSender = scope.ServiceProvider.GetService<ITopicEventSender>();
        var updatedUser = await userService.UpdateUserStatusAsync(userId, isDeleted);

        await eventSender.SendAsync("OnUserStatusChanged", updatedUser);

        return updatedUser;
    }
}