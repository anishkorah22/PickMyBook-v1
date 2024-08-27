using Experion.PickMyBook.Business.Service.IService;
using Experion.PickMyBook.Data;
using Experion.PickMyBook.Infrastructure;
using Experion.PickMyBook.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Experion.PickMyBook.Business.Services
{
    public class UserService:IUserService
    {
        private readonly LibraryContext _context;
        private readonly IRepository<User> _userRepository;

        public UserService(IRepository<User> userRepository ,LibraryContext context)
        {
            _userRepository = userRepository;
            _context = context;
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _userRepository.GetAllAsync();
        }

        public async Task<User> GetUserByIdAsync(int id)
        {
            return await _userRepository.GetByIdAsync(id);
        }

        public async Task<User> CreateUserAsync(string userName, IEnumerable<string> roles)
        {
            var newUser = new User
            {
                UserName = userName,
                Roles = roles,
                IsDeleted = false,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await _userRepository.AddAsync(newUser);
            return newUser;
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
    }
}