using Experion.PickMyBook.Infrastructure.Models;
using Experion.PickMyBook.Infrastructure.Models.DTO;
using Experion.PickMyBook.Data.IRepository;
using Experion.PickMyBook.Business.Service.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class BookService : IBookService
{
    private readonly IBookRepository _bookRepository;

    public BookService(IBookRepository bookRepository)
    {
        _bookRepository = bookRepository;
    }

    public async Task<Book> AddBookAsync(AddBooksDTO dto)
    {
        var book = new Book
        {
            Title = dto.Title,
            Author = dto.Author,
            ISBN = dto.ISBN,
            Publisher = dto.Publisher,
            AvailableCopies = dto.AvailableCopies,
            PublishedYear = dto.PublishedYear,
            Genre = dto.Genre,
            CreatedAt = DateTime.UtcNow // Ensure CreatedAt is set
        };

        await _bookRepository.AddAsync(book);
        return book;
    }

    public async Task<Book> UpdateBookAsync(Book book)
    {
        var existingBook = await _bookRepository.GetByIdAsync(book.BookId);

        if (existingBook == null)
        {
            throw new KeyNotFoundException("Book not found.");
        }

        existingBook.Title = !string.IsNullOrEmpty(book.Title) ? book.Title : existingBook.Title;
        existingBook.Author = !string.IsNullOrEmpty(book.Author) ? book.Author : existingBook.Author;
        existingBook.ISBN = !string.IsNullOrEmpty(book.ISBN) ? book.ISBN : existingBook.ISBN;
        existingBook.Publisher = !string.IsNullOrEmpty(book.Publisher) ? book.Publisher : existingBook.Publisher;
        existingBook.AvailableCopies = book.AvailableCopies >= 0 ? book.AvailableCopies : existingBook.AvailableCopies; // Ensure valid values
        existingBook.PublishedYear = book.PublishedYear > 0 ? book.PublishedYear : existingBook.PublishedYear;
        existingBook.Genre = !string.IsNullOrEmpty(book.Genre) ? book.Genre : existingBook.Genre;
        existingBook.UpdatedAt = DateTime.UtcNow;

        await _bookRepository.UpdateAsync(existingBook);
        return existingBook;
    }

    public async Task DeleteBookAsync(int id)
    {
        var book = await _bookRepository.GetByIdAsync(id);
        if (book == null)
        {
            throw new KeyNotFoundException("Book not found.");
        }

        await _bookRepository.DeleteAsync(id);
    }

    public async Task<int> GetTotalBooksCountAsync()
    {
        var books = await _bookRepository.GetAllAsync();
        return books
            .Where(b => !b.IsDeleted.HasValue || !b.IsDeleted.Value)
            .Sum(b => b.AvailableCopies ?? 0);
    }

    public async Task<Book> UpdateBookStatusAsync(int bookId, bool isDeleted)
    {
        var book = await _bookRepository.GetBookByIdAsync(bookId);

        if (book == null)
        {
            throw new ArgumentException("Book not found.");
        }

        book.IsDeleted = isDeleted;
        book.UpdatedAt = DateTime.UtcNow;
        await _bookRepository.UpdateBookAsync(book);
        return book;
    }

}
