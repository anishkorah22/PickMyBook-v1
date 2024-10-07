using Experion.PickMyBook.Infrastructure.Models;
namespace Experion.PickMyBook.Business.Service.IService
{
    public interface IUserService
    {
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<IEnumerable<User>> GetUsersAsync();
        Task<int> GetTotalActiveUsersCountAsync();
        Task<User> GetUserByIdAsync(int id);
        Task<User> CreateUserAsync(User user, string roleString);
        Task UpdateUserAsync(User user);
        Task DeleteUserAsync(int id);
        Task<User> UpdateUserStatusAsync(int userId, bool isDeleted);
    }
}
