using Experion.PickMyBook.Business.Service.IService;
using Experion.PickMyBook.Data;
using Experion.PickMyBook.Data.IRepository;
using Experion.PickMyBook.Infrastructure;
using Experion.PickMyBook.Infrastructure.Models;
using Experion.PickMyBook.Infrastructure.Models.DTO;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Experion.PickMyBook.Business.Service
{
    public class BorrowingService : IBorrowingService
    {
        private readonly LibraryContext _context;
        private readonly IBorrowingsRepository _borrowingsRepository;

        public BorrowingService(LibraryContext context, IBorrowingsRepository borrowingsRepository)
        {
            _context = context;
            _borrowingsRepository = borrowingsRepository;
        }

        public async Task<IEnumerable<Borrowings>> GetBorrowingsAsync()
        {
            return await _borrowingsRepository.GetAllAsync();
        }
        private decimal CalculateFineAmount(DateTime returnDate, DateTime currentDate)
        {
            var overdueDays = (currentDate - returnDate).Days;
            return overdueDays > 0 ? 5 * overdueDays : 0;
        }

        public async Task<Borrowings> BorrowBookAsync(int userId, int bookId)
        {
            var currentDate = DateTime.UtcNow;
            var book = await _context.Books.FindAsync(bookId);
            if (book == null || book.AvailableCopies <= 0)
            {
                throw new ArgumentException("Book not available or not found.");
            }

            var user = await _context.Users.FindAsync(userId);
            if (user == null)
            {
                throw new ArgumentException("User not found.");
            }

            var existingBorrowing = await _context.Borrowings
                .Where(b => b.BookId == bookId && b.UserId == userId && b.Status == BorrowingStatus.Borrowed)
                .FirstOrDefaultAsync();

            if (existingBorrowing != null)
            {
                throw new InvalidOperationException("This book is already borrowed by the user.");
            }

            var borrowing = new Borrowings
            {
                BookId = bookId,
                UserId = userId,
                BorrowDate = currentDate,
                ReturnDate = currentDate.AddDays(14),
                Status = BorrowingStatus.Borrowed 
            };

            _context.Borrowings.Add(borrowing);
            book.AvailableCopies -= 1;
            _context.Books.Update(book);
            await _context.SaveChangesAsync();

            return borrowing;
        }

        public async Task<Borrowings> UpdateBorrowingAsync(Borrowings borrowing)
        {
            var existingBorrowing = await _context.Borrowings
                .FirstOrDefaultAsync(b => b.BookId == borrowing.BookId && b.UserId == borrowing.UserId);

            if (existingBorrowing == null)
            {
                throw new KeyNotFoundException("Borrowing record not found.");
            }


            existingBorrowing.ReturnDate = borrowing.ReturnDate.HasValue ? borrowing.ReturnDate : existingBorrowing.ReturnDate;
            existingBorrowing.Status = borrowing.Status;  // Use the enum directly
            existingBorrowing.FineAmt = borrowing.FineAmt.HasValue ? borrowing.FineAmt : existingBorrowing.FineAmt;


            _context.Borrowings.Update(existingBorrowing);
            await _context.SaveChangesAsync();

            return existingBorrowing;
        }

        public async Task<Borrowings> ReturnBookAsync(int userId, int bookId)
        {
            var currentDate = DateTime.UtcNow;

            var borrowing = await _context.Borrowings
                .FirstOrDefaultAsync(b => b.BookId == bookId && b.UserId == userId && b.Status == BorrowingStatus.Borrowed);

            if (borrowing == null)
            {
                throw new ArgumentException("Borrowing record not found.");
            }

            borrowing.ReturnDate = currentDate;

            if (currentDate > borrowing.ReturnDate)
            {
                borrowing.FineAmt = CalculateFineAmount(borrowing.ReturnDate.Value, currentDate);
            }

            borrowing.Status = BorrowingStatus.Returned;  // Use the enum here

            var book = await _context.Books.FindAsync(bookId);
            if (book == null)
            {
                throw new ArgumentException("Book not found.");
            }
            book.AvailableCopies += 1;
            _context.Books.Update(book);

            await _context.SaveChangesAsync();

            return borrowing;
        }

        public async Task<int> GetTotalBorrowingsCountAsync()
        {
            return await _context.Borrowings

               .Where(b => b.Status == BorrowingStatus.Borrowed)  // Use the enum here
               .CountAsync();
        }


        public async Task<List<Borrowings>> GetBorrowingsByUserIdAsync(int userId)
        {
            return await _borrowingsRepository.GetBorrowingsByUserIdAsync(userId);
        }

       public async Task<List<UserBooksReadInfoDTO>> GetBooksReadByUserAsync(int userId)
{
    var user = await _context.Users.FindAsync(userId);
    if (user == null)
    {
        throw new ArgumentException("User not found.");
    }

    var borrowings = await _context.Borrowings
        .Where(b => b.UserId == userId && b.Status == BorrowingStatus.Returned)
        .Include(b => b.Book) // Ensure Book entity is loaded
        .ToListAsync();

    var booksRead = borrowings
        .Select(b => new UserBooksReadInfoDTO
        {
            BooksReadCount = borrowings.Count(br => br.Status == BorrowingStatus.Returned),
            Title = b.Book?.Title ?? "Unknown Title", // Handle potential nulls
            Author = b.Book?.Author ?? "Unknown Author", // Handle potential nulls
            BorrowDate = b.BorrowDate.HasValue ? DateOnly.FromDateTime(b.BorrowDate.Value) : DateOnly.MinValue,
            ReturnDate = b.ReturnDate.HasValue ? DateOnly.FromDateTime(b.ReturnDate.Value) : DateOnly.MinValue

        })
        .ToList();

    return booksRead;
}



    }
}
