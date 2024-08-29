using Experion.PickMyBook.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Experion.PickMyBook.Business.Service.IService
{
    public interface IUserService
    {
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<int> GetTotalActiveUsersCountAsync();
        Task<User> GetUserByIdAsync(int id);
        Task<User> CreateUserAsync(string userName, IEnumerable<string> roles);
        Task UpdateUserAsync(User user);
        Task DeleteUserAsync(int id);
        Task<User> UpdateUserStatusAsync(int userId, bool isDeleted);






    }
}
