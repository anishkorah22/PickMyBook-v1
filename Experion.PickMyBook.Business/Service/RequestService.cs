using Experion.PickMyBook.Data;
using Experion.PickMyBook.Data.IRepository;
using Experion.PickMyBook.Infrastructure.Models;
using Experion.PickMyBook.Infrastructure.Models.DTO;

public class RequestService : IRequestService
{
    private readonly IRequestRepository _requestRepository;
    private readonly IBookRepository _bookRepository;
    public RequestService(IRequestRepository requestRepository, IBookRepository BookRepository)
    {
        _requestRepository = requestRepository;
        _bookRepository = BookRepository;
    }

    public async Task<IEnumerable<RequestDTO>> GetAllRequestsAsync()
    {

        var request = new List<Request>();

        request = (await _requestRepository.GetAllRequestsAsync()).ToList();

        return request.Select(r => new RequestDTO
        {
            BookTitle = r.Book.Title,
            Username = r.User.UserName,
            RequestType = r.RequestType.ToString()
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
        // Retrieve the request
        var request = await _requestRepository.GetRequestByIdAsync(requestId)
                      ?? throw new InvalidOperationException("Request cannot be approved.");

        // Update the request status to Approved
        request.Status = RequestStatus.Approved;
        await _requestRepository.UpdateRequestAsync(request);

        // Process the request based on its type
        switch (request.RequestType)
        {
            case RequestType.BorrowRequest:
                // Create a new borrowing record
                await _requestRepository.AddBorrowingAsync(new Borrowings
                {
                    BookId = request.BookId,
                    UserId = request.UserId,
                    BorrowDate = DateTime.UtcNow,
                    ReturnDate = DateTime.UtcNow.AddDays(14),
                    Status = BorrowingStatus.Borrowed
                });

                // Update the available copies
                var book = await _bookRepository.GetBookByIdAsync(request.BookId)
                            ?? throw new InvalidOperationException("Book not found.");

                if (book.AvailableCopies <= 0)
                {
                    throw new InvalidOperationException("No available copies left.");
                }

                book.AvailableCopies--;
                await _bookRepository.UpdateBookAsync(book);
                break;

            case RequestType.ReturnRequest:
                // Retrieve the existing borrowing record
                var borrowing = await _requestRepository.GetBorrowingByBookAndUserAsync(request.BookId, request.UserId)
                                ?? throw new InvalidOperationException("No borrowing record found for this book and user.");

                // Update the borrowing record to returned
                borrowing.Status = BorrowingStatus.Returned;
                borrowing.ReturnDate = DateTime.UtcNow;
                await _requestRepository.UpdateBorrowingAsync(borrowing);

                // Update the available copies
                var returnedBook = await _bookRepository.GetBookByIdAsync(request.BookId)
                                   ?? throw new InvalidOperationException("Book not found.");

                returnedBook.AvailableCopies++;
                await _bookRepository.UpdateBookAsync(returnedBook);
                break;

            default:
                throw new InvalidOperationException("Unsupported request type.");
        }

        return request;
    }

    public async Task<Request> DeclineRequestAsync(int requestId, string message)
    {
        var request = await _requestRepository.GetRequestByIdAsync(requestId);
        if (request == null || request.Status != RequestStatus.Pending)
        {
            throw new InvalidOperationException("Request cannot be declined.");
        }

        request.Status = RequestStatus.Declined;
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
                RequestType = r.RequestType.ToString(),
                RequestStatus = r.Status.ToString(),
                Message = r.Message ?? "No message"
            })
            .ToList();

        return userRequests;
    }

}
