using Experion.PickMyBook.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Experion.PickMyBook.Business.Service
{
    public interface IBorrowingService
    {
        Task<Borrowings> BorrowBookAsync(int userId, int bookId);
        Task<Borrowings> UpdateBorrowingAsync(Borrowings borrowing);
        Task<Borrowings> ReturnBookAsync(int userId, int bookId);
    }

    public class BorrowingService : IBorrowingService
    {
        private readonly LibraryContext _context;

        public BorrowingService(LibraryContext context)
        {
            _context = context;
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
            if (book == null)
            {
                throw new ArgumentException("Book not found.");
            }

            var user = await _context.Users.FindAsync(userId);
            if (user == null)
            {
                throw new ArgumentException("User not found.");
            }

            var existingBorrowing = await _context.Borrowings
                .Where(b => b.BookId == bookId && b.UserId == userId)
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
                Status = "Borrowed"
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
            existingBorrowing.Status = !string.IsNullOrEmpty(borrowing.Status) ? borrowing.Status : existingBorrowing.Status;
            existingBorrowing.FineAmt = borrowing.FineAmt.HasValue ? borrowing.FineAmt : existingBorrowing.FineAmt;

            _context.Borrowings.Update(existingBorrowing);
            await _context.SaveChangesAsync();

            return existingBorrowing;
        }

        public async Task<Borrowings> ReturnBookAsync(int userId, int bookId)
        {
            var currentDate = DateTime.UtcNow;

            var borrowing = await _context.Borrowings
                .FirstOrDefaultAsync(b => b.BookId == bookId && b.UserId == userId);

            if (borrowing == null)
            {
                throw new ArgumentException("Borrowing record not found.");
            }

            borrowing.ReturnDate = currentDate;

            if (currentDate > borrowing.ReturnDate)
            {
                borrowing.FineAmt = CalculateFineAmount((DateTime)borrowing.ReturnDate, currentDate);
            }

            borrowing.Status = "Returned";

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
    }

}
