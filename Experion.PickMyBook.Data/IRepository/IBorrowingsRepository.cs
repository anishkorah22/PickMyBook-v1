using Experion.PickMyBook.Infrastructure.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Experion.PickMyBook.Data.IRepository
{
    public interface IBorrowingsRepository
    {
        Task<IEnumerable<Borrowings>> GetAllAsync();
        Task<Borrowings> GetByIdAsync(int id);
        Task AddAsync(Borrowings entity);
        Task UpdateAsync(Borrowings entity);
        Task DeleteAsync(int id);
        Task<List<Borrowings>> GetBorrowingsByUserIdAsync(int userId);
    }
}
