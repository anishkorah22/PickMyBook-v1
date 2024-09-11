using Experion.PickMyBook.Data.IRepository;
using Experion.PickMyBook.Infrastructure;
using Experion.PickMyBook.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Experion.PickMyBook.Data
{
    public class BorrowingsRepository : IBorrowingsRepository
    {
        private readonly LibraryContext _context;

        public BorrowingsRepository(LibraryContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Borrowings>> GetAllAsync()
        {
            return await _context.Borrowings
                .Include(b => b.User) 
                .Include(b => b.Book) 
                .ToListAsync();
        }

        public async Task<Borrowings> GetByIdAsync(int id)
        {
            return await _context.Borrowings.FindAsync(id);
        }

        public async Task AddAsync(Borrowings entity)
        {
            await _context.Borrowings.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Borrowings entity)
        {
            _context.Borrowings.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var borrowing = await _context.Borrowings.FindAsync(id);
            if (borrowing != null)
            {
                _context.Borrowings.Remove(borrowing);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<Borrowings>> GetBorrowingsByUserIdAsync(int userId)
        {
            return await _context.Borrowings
                .Where(b => b.UserId == userId)
                .ToListAsync();
        }

    }
}
