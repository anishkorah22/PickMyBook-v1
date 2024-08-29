using Experion.PickMyBook.Infrastructure.Models;

public interface IRequestRepository
{
    Task<IEnumerable<Request>> GetAllRequestsAsync();
}