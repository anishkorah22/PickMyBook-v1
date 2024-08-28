using Experion.PickMyBook.Infrastructure.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Experion.PickMyBook.Data.IRepository
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAllAsync();
        Task<User> GetByIdAsync(int id);
        Task AddAsync(User entity);
        Task UpdateAsync(User entity);
        Task DeleteAsync(int id);
    }
}
