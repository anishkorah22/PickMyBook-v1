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
}
