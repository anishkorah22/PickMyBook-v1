using Experion.PickMyBook.Infrastructure.Models;

public interface IRequestRepository
{
    Task<IEnumerable<Request>> GetAllRequestsAsync();
    Task<Request> CreateBorrowRequest(int bookId, int userId);
    Task<Request> CreateReturnRequestAsync(int bookId, int userId);
    Task<Request> GetRequestByIdAsync(int requestId);
    Task UpdateRequestAsync(Request request);
    Task AddBorrowingAsync(Borrowings borrowing);
    Task<Borrowings> GetBorrowingByBookAndUserAsync(int bookId, int userId);
    Task UpdateBorrowingAsync(Borrowings borrowing);

}