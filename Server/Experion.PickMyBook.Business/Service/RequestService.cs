using Experion.PickMyBook.Data;
using Experion.PickMyBook.Data.IRepository;
using Experion.PickMyBook.Infrastructure.Models;
using Experion.PickMyBook.Infrastructure.Models.DTO;

public class RequestService : IRequestService
{
    private readonly IRequestRepository _requestRepository;
    private readonly IBookRepository _bookRepository;

    public RequestService(IRequestRepository requestRepository, IBookRepository bookRepository)
    {
        _requestRepository = requestRepository;
        _bookRepository = bookRepository;
    }

    public async Task<IEnumerable<RequestDTO>> GetAllRequestsAsync()
    {
        var requests = await _requestRepository.GetAllRequestsAsync();

        return requests.Select(r => new RequestDTO
        {
            BookTitle = r.Book.Title,
            Username = r.User.UserName,
            RequestType = ((RequestTypeValue)r.RequestTypeValue).ToString()
        });
    }

    public async Task<Request> CreateBorrowRequestAsync(int bookId, int userId)
    {
        return await _requestRepository.CreateBorrowRequest(bookId, userId);
    }

    public async Task<Request> CreateReturnRequestAsync(int bookId, int userId)
    {
        return await _requestRepository.CreateReturnRequestAsync(bookId, userId);
    }

    public async Task<Request> ApproveRequestAsync(int requestId)
    {
        var request = await _requestRepository.GetRequestByIdAsync(requestId);
        if (request == null)
        {
            throw new InvalidOperationException("Request not found.");
        }

        request.RequestStatusValue = (int)RequestStatusValue.Approved;
        await _requestRepository.UpdateRequestAsync(request);

        switch ((RequestTypeValue)request.RequestTypeValue)
        {
            case RequestTypeValue.BorrowRequest:
                await ProcessApprovedBorrowRequest(request);
                break;

            case RequestTypeValue.ReturnRequest:
                await ProcessApprovedReturnRequest(request);
                break;

            default:
                throw new InvalidOperationException("Unsupported request type.");
        }

        return request;
    }

    private async Task ProcessApprovedBorrowRequest(Request request)
    {
        await _requestRepository.AddBorrowingAsync(new Borrowings
        {
            BookId = request.BookId,
            UserId = request.UserId,
            BorrowDate = DateTime.UtcNow,
            ReturnDate = DateTime.UtcNow.AddDays(14),
            BorrowingStatusValue = (int)BorrowingStatusValue.Borrowed
        });

        var book = await _bookRepository.GetBookByIdAsync(request.BookId)
                    ?? throw new InvalidOperationException("Book not found.");

        if (book.AvailableCopies <= 0)
        {
            throw new InvalidOperationException("No available copies left.");
        }

        book.AvailableCopies--;
        await _bookRepository.UpdateBookAsync(book);
    }

    private async Task ProcessApprovedReturnRequest(Request request)
    {
        var borrowing = await _requestRepository.GetBorrowingByBookAndUserAsync(request.BookId, request.UserId)
                        ?? throw new InvalidOperationException("No borrowing record found for this book and user.");

        borrowing.BorrowingStatusValue = (int)BorrowingStatusValue.Returned; // Assuming 'Pending' for return
        borrowing.ReturnDate = DateTime.UtcNow;
        await _requestRepository.UpdateBorrowingAsync(borrowing);

        var returnedBook = await _bookRepository.GetBookByIdAsync(request.BookId)
                           ?? throw new InvalidOperationException("Book not found.");

        returnedBook.AvailableCopies++;
        await _bookRepository.UpdateBookAsync(returnedBook);
    }

    public async Task<Request> DeclineRequestAsync(int requestId, string message)
    {
        var request = await _requestRepository.GetRequestByIdAsync(requestId);
        if (request == null || request.RequestStatusValue != (int)RequestStatusValue.Pending)
        {
            throw new InvalidOperationException("Request cannot be declined.");
        }

        request.RequestStatusValue = (int)RequestStatusValue.Declined;
        request.Message = message;

        await _requestRepository.UpdateRequestAsync(request);
        return request;
    }

    public async Task<IEnumerable<UserRequestsDTO>> GetRequestsByUserAsync(int userId)
    {
        var requests = await _requestRepository.GetAllRequestsAsync();

        var userRequests = requests
            .Where(r => r.UserId == userId)
            .Select(r => new UserRequestsDTO
            {
                BookTitle = r.Book?.Title ?? "Unknown Title",
                RequestType = ((RequestTypeValue)r.RequestTypeValue).ToString(),
                RequestStatus = ((RequestStatusValue)r.RequestStatusValue).ToString(),
                Message = r.Message ?? "No message"
            })
            .ToList();

        return userRequests;
    }
}
