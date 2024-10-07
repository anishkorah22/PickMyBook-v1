using Experion.PickMyBook.Business.Service.IService;
using Experion.PickMyBook.Data;
using Experion.PickMyBook.Data.IRepository;
using Experion.PickMyBook.Infrastructure;
using Experion.PickMyBook.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace Experion.PickMyBook.Business.Services
{
    public class UserService:IUserService
    {
        private readonly LibraryContext _context;
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository ,LibraryContext context)
        {
            _userRepository = userRepository;
            _context = context;
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _userRepository.GetAllAsync();
        }
        public async Task<IEnumerable<User>> GetUsersAsync()
        {
            return await _userRepository.GetUsersAsync();
        }

        public async Task<User> GetUserByIdAsync(int id)
        {
            return await _userRepository.GetByIdAsync(id);
        }

        public async Task<User> CreateUserAsync(User user, string roleTypeValue)
        {

            if (!Enum.TryParse(roleTypeValue, true, out RoleTypeValue parsedRoleType))
            {
                throw new ArgumentException($"Invalid role type value: {roleTypeValue}");
            }

            var role = await _context.Roles.FirstOrDefaultAsync(r => r.RoleTypeId == (int)parsedRoleType);
            if (role == null)
            {
                throw new ArgumentException($"Role not found for RoleTypeValue: {roleTypeValue}");
            }

            user.RoleTypeId = role.RoleTypeId;
            user.Role = role;
            user.IsDeleted = false;
            user.CreatedAt = DateTime.UtcNow;
            user.UpdatedAt = DateTime.UtcNow;

            await _userRepository.AddAsync(user);
            return user;
        }

        public async Task UpdateUserAsync(User user)
        {
            await _userRepository.UpdateAsync(user);
        }

        public async Task DeleteUserAsync(int id)
        {
            await _userRepository.DeleteAsync(id);
        }

        public async Task<int> GetTotalActiveUsersCountAsync()
        {
            return await _context.Users
                .Where(u => !u.IsDeleted)
                .CountAsync();
        }

        public async Task<User> UpdateUserStatusAsync(int userId, bool isDeleted)
        {
            var user = await _userRepository.GetByUserIdAsync(userId);
            if (user == null)
            {
                throw new ArgumentException("User not found.");
            }
            user.IsDeleted = isDeleted;
            user.UpdatedAt = DateTime.UtcNow;
            await _userRepository.UpdateUserAsync(user);
            return user;
        }
    }
}