using Experion.PickMyBook.Infrastructure.Models;
using Experion.PickMyBook.Infrastructure.Models.DTO;

public class RequestService : IRequestService
{
    private readonly IRequestRepository _requestRepository;

    public RequestService(IRequestRepository requestRepository)
    {
        _requestRepository = requestRepository;
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
