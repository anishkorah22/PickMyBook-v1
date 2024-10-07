﻿using Experion.PickMyBook.Data.IRepository;
using Experion.PickMyBook.Infrastructure;
using Experion.PickMyBook.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Experion.PickMyBook.Data
{
    public class BookRepository : IBookRepository
    {
        private readonly LibraryContext _context;

        public BookRepository(LibraryContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Book>> GetAllAsync()
        {
            // Fetch books only if they are not marked as deleted
            return await _context.Books
                .Where(b => !b.IsDeleted.HasValue || !b.IsDeleted.Value)
                .ToListAsync();
        }

        public async Task<IEnumerable<Book>> GetBooksAsync()
        {
            // Fetch books only if they are not marked as deleted
            return await _context.Books
                .ToListAsync();
        }

        public async Task<Book> GetByIdAsync(int id)
        {
            // Include check for IsDeleted
            return await _context.Books
                .Where(b => b.BookId == id && (!b.IsDeleted.HasValue || !b.IsDeleted.Value))
                .FirstOrDefaultAsync();
        }

        public async Task AddAsync(Book entity)
        {
            // Add book to the context
            await _context.Books.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Book entity)
        {
            var existingBook = await _context.Books.FindAsync(entity.BookId);
            if (existingBook == null || existingBook.IsDeleted == true)
            {
                throw new KeyNotFoundException("Book not found or is deleted.");
            }

            // Update properties if the book is found and not deleted
            _context.Entry(existingBook).CurrentValues.SetValues(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book == null || book.IsDeleted == true)
            {
                throw new KeyNotFoundException("Book not found or is already deleted.");
            }

            book.IsDeleted = true;
            _context.Books.Update(book);
            await _context.SaveChangesAsync();
        }

        public async Task<Book> GetBookByIdAsync(int bookId)
        {
            return await _context.Books.FindAsync(bookId);
        }

        public async Task UpdateBookAsync(Book book)
        {
            _context.Books.Update(book);
            await _context.SaveChangesAsync();
        }
    }
}
