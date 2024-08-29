public interface IRequestService
{
    Task<IEnumerable<RequestDTO>> GetAllRequestsAsync();
}
