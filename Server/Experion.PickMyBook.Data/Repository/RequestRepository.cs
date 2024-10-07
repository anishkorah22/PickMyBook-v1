using Experion.PickMyBook.Infrastructure.Models;
using Experion.PickMyBook.Infrastructure;
using Microsoft.EntityFrameworkCore;
public class RequestRepository : IRequestRepository
{
    private readonly LibraryContext _context;

    public RequestRepository(LibraryContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Request>> GetAllRequestsAsync()
    {
        return await _context.Requests
            .Include(r => r.Book)
            .Include(r => r.User)
            .Include(r => r.RequestType)
            .Include(r => r.RequestStatus)
            .ToListAsync();
    }

    public async Task<Request> CreateBorrowRequest(int bookId, int userId)
    {
        var newRequest = new Request
        {
            BookId = bookId,
            UserId = userId,
            RequestTypeValue = (int)RequestTypeValue.BorrowRequest,
            RequestStatusValue = (int)RequestStatusValue.Pending,
            RequestedAt = DateTime.UtcNow
        };

        _context.Requests.Add(newRequest);
        await _context.SaveChangesAsync();
        return newRequest;
    }

    public async Task<Request> CreateReturnRequestAsync(int bookId, int userId)
    {
        var borrowRequest = await _context.Requests
            .FirstOrDefaultAsync(r => r.BookId == bookId &&
                                      r.UserId == userId &&
                                      r.RequestStatusValue == (int)RequestStatusValue.Approved &&
                                      r.RequestTypeValue == (int)RequestTypeValue.BorrowRequest);

        if (borrowRequest == null)
        {
            throw new InvalidOperationException("No approved borrow request found for this book and user.");
        }

        // Create a new return request
        var returnRequest = new Request
        {
            BookId = bookId,
            UserId = userId,
            RequestTypeValue = (int)RequestTypeValue.ReturnRequest,
            RequestStatusValue = (int)RequestStatusValue.Pending,
            RequestedAt = DateTime.UtcNow
        };

        _context.Requests.Add(returnRequest);
        await _context.SaveChangesAsync();
        return returnRequest;
    }
    public async Task<Request> GetRequestByIdAsync(int requestId)
    {
        // Check if the request exists
        var request = await _context.Requests
            .Where(r => r.RequestId == requestId)
            .Include(r => r.Book)
            .Include(r => r.User)
            .Include(r => r.RequestType)
            .Include(r => r.RequestStatus)
            .FirstOrDefaultAsync();

        return request;
    }

    public async Task UpdateRequestAsync(Request request)
    {
        _context.Requests.Update(request);
        await _context.SaveChangesAsync();
    }

    public async Task AddBorrowingAsync(Borrowings borrowing)
    {
        _context.Borrowings.Add(borrowing);
        await _context.SaveChangesAsync();
    }

    public async Task<Borrowings> GetBorrowingByBookAndUserAsync(int bookId, int userId)
    {
        
        return await _context.Borrowings
            .Include(b => b.BorrowingStatus)
            .FirstOrDefaultAsync(b => b.BookId == bookId && b.UserId == userId && b.BorrowingStatusValue == (int)BorrowingStatusValue.Borrowed);
    }

    public async Task UpdateBorrowingAsync(Borrowings borrowing)
    {
        _context.Borrowings.Update(borrowing);
        await _context.SaveChangesAsync();
    }
}