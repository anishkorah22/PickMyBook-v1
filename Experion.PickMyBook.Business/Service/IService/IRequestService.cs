using Experion.PickMyBook.Infrastructure.Models.DTO;

public interface IRequestService
{
    Task<IEnumerable<RequestDTO>> GetAllRequestsAsync();

    Task<IEnumerable<UserRequestsDTO>> GetRequestsByUserAsync(int userId);
}
