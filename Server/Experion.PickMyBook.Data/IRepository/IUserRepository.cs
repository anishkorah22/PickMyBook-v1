using Experion.PickMyBook.Infrastructure.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Experion.PickMyBook.Data.IRepository
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAllAsync();
        Task<IEnumerable<User>> GetUsersAsync();
        Task<User> GetByIdAsync(int id);
        Task AddAsync(User entity);
        Task UpdateAsync(User entity);
        Task DeleteAsync(int id);
        Task UpdateStatusAsync(User entity);
        Task<User> GetByUserIdAsync(int id);
        Task UpdateUserAsync(User user);



    }
}
