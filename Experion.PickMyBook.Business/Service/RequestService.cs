using Experion.PickMyBook.Infrastructure.Models;

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
}
