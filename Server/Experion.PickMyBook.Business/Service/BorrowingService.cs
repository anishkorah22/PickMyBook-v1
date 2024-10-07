using Experion.PickMyBook.Business.Service.IService;
using Experion.PickMyBook.Data;
using Experion.PickMyBook.Data.IRepository;
using Experion.PickMyBook.Infrastructure;
using Experion.PickMyBook.Infrastructure.Models;
using Experion.PickMyBook.Infrastructure.Models.DTO;
using Microsoft.EntityFrameworkCore;


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
                .FirstOrDefaultAsync(b => b.BookId == bookId && b.UserId == userId && b.BorrowingStatusValue == (int)BorrowingStatusValue.Borrowed);
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
                BorrowingStatusValue = (int)BorrowingStatusValue.Borrowed
            };

            await _borrowingsRepository.AddAsync(borrowing);

            book.AvailableCopies -= 1;
            _context.Books.Update(book);
            await _context.SaveChangesAsync();

            return borrowing;
        }

        public async Task<Borrowings> UpdateBorrowingAsync(Borrowings borrowing)
        {
            var existingBorrowing = await _borrowingsRepository.GetByIdAsync(borrowing.BookId);
            if (existingBorrowing == null)
            {
                throw new KeyNotFoundException("Borrowing record not found.");
            }

            existingBorrowing.ReturnDate = borrowing.ReturnDate ?? existingBorrowing.ReturnDate;
            existingBorrowing.BorrowingStatusValue = borrowing.BorrowingStatusValue;
            existingBorrowing.FineAmt = borrowing.FineAmt ?? existingBorrowing.FineAmt;

            await _borrowingsRepository.UpdateAsync(existingBorrowing);
            return existingBorrowing;
        }

        public async Task<Borrowings> ReturnBookAsync(int userId, int bookId)
        {
            var currentDate = DateTime.UtcNow;
            var borrowing = await _context.Borrowings
                .FirstOrDefaultAsync(b => b.BookId == bookId && b.UserId == userId && b.BorrowingStatusValue == (int)BorrowingStatusValue.Borrowed);
            if (borrowing == null)
            {
                throw new ArgumentException("Borrowing record not found.");
            }

            borrowing.ReturnDate = currentDate;
            if (currentDate > borrowing.ReturnDate)
            {
                borrowing.FineAmt = CalculateFineAmount(borrowing.ReturnDate.Value, currentDate);
            }
            borrowing.BorrowingStatusValue = (int)BorrowingStatusValue.Returned;  // Assuming 'Pending' for return approval

            var book = await _context.Books.FindAsync(bookId);
            if (book == null)
            {
                throw new ArgumentException("Book not found.");
            }

            book.AvailableCopies += 1;
            _context.Books.Update(book);

            await _borrowingsRepository.UpdateAsync(borrowing);
            return borrowing;
        }

        public async Task<int> GetTotalBorrowingsCountAsync()
        {
            var borrowings = await _borrowingsRepository.GetAllAsync();
            return borrowings.Count(b => b.BorrowingStatusValue == (int)BorrowingStatusValue.Borrowed);
        }


        public async Task<List<Borrowings>> GetBorrowingsByUserIdAsync(int userId)
        {
            return await _context.Borrowings
       .Include(b => b.Book) 
       .Where(b => b.UserId == userId)
       .ToListAsync();
        }

       public async Task<List<UserBooksReadInfoDTO>> GetBooksReadByUserAsync(int userId)
       {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
            {
                throw new ArgumentException("User not found.");
            }

            var borrowings = await _context.Borrowings
        .Include(b => b.Book)
        .Where(b => b.UserId == userId)
        .ToListAsync();
            var booksRead = borrowings
                .Where(b => b.BorrowingStatusValue == (int)BorrowingStatusValue.Returned && b.ReturnDate.HasValue)
                .Select(b => new UserBooksReadInfoDTO
                {
                    BooksReadCount = borrowings.Count(br => br.BorrowingStatusValue == (int)BorrowingStatusValue.Returned && br.ReturnDate.HasValue),
                    Title = b.Book?.Title ?? "Unknown Title",
                    Author = b.Book?.Author ?? "Unknown Author",
                    BorrowDate = b.BorrowDate.HasValue ? DateOnly.FromDateTime(b.BorrowDate.Value) : DateOnly.MinValue,
                    ReturnDate = b.ReturnDate.HasValue ? DateOnly.FromDateTime(b.ReturnDate.Value) : DateOnly.MinValue
                })
                .ToList();

            return booksRead;
        }



    }
}
