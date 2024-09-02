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
            .ToListAsync();
    }

    public async Task<Request> CreateBorrowRequest(int bookId, int userId)
    {
        var newRequest = new Request
        {
            BookId = bookId,
            UserId = userId,
            RequestType = RequestType.BorrowRequest,
        };

        _context.Requests.Add(newRequest);
        await _context.SaveChangesAsync();
        return newRequest;
    }

    public async Task<Request> CreateReturnRequestAsync(int bookId, int userId)
    {
        var borrowingRequest = await _context.Requests
            .FirstOrDefaultAsync(r => r.BookId == bookId && r.UserId == userId && r.Status == RequestStatus.Approved && r.RequestType == RequestType.BorrowRequest);

        if (borrowingRequest == null)
        {
            throw new InvalidOperationException("No approved borrow request found for this book and user.");
        }

        // Updating the request
        borrowingRequest.RequestType = RequestType.ReturnRequest;
        borrowingRequest.Status = RequestStatus.Pending;
        borrowingRequest.RequestedAt = DateTime.UtcNow;

        _context.Requests.Update(borrowingRequest);
        await _context.SaveChangesAsync();
        return borrowingRequest;
    }

    public async Task<Request> GetRequestByIdAsync(int requestId)
    {
        // Check if the request exists
        var request = await _context.Requests
            .Where(r => r.RequestId == requestId)
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
            .FirstOrDefaultAsync(b => b.BookId == bookId && b.UserId == userId && b.Status == BorrowingStatus.Borrowed);
    }
    public async Task UpdateBorrowingAsync(Borrowings borrowing)
    {
        _context.Borrowings.Update(borrowing);
        await _context.SaveChangesAsync();
    }

    

}
