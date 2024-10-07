using Experion.PickMyBook.Infrastructure.Models;
using Experion.PickMyBook.Infrastructure.Models.DTO;
using Experion.PickMyBook.Data.IRepository;
using Experion.PickMyBook.Business.Service.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Hosting;
using HotChocolate.Types;
using Experion.PickMyBook.Data;

public class BookService : IBookService
{
    private readonly IBookRepository _bookRepository;
    private readonly IWebHostEnvironment _environment;

    public BookService(IBookRepository bookRepository, IWebHostEnvironment environment)
    {
        _bookRepository = bookRepository;
        _environment = environment;
        if (string.IsNullOrEmpty(_environment.WebRootPath))
        {
            throw new InvalidOperationException("WebRootPath is not configured.");
        }
    }
    public async Task<IEnumerable<Book>> GetBooksAsync()
    {
        return await _bookRepository.GetAllAsync();
    }
    public async Task<IEnumerable<Book>> GetAllBooksAsync()
    {
        return await _bookRepository.GetBooksAsync();
    }
    public async Task<Book> AddBookAsync(Book book)
    {

        var newBook = new Book
        {
            Title = book.Title,
            Author = book.Author,
            ISBN = book.ISBN,
            Publisher = book.Publisher,
            AvailableCopies = book.AvailableCopies,
            PublishedYear = book.PublishedYear,
            Genre = book.Genre,
            CreatedAt = DateTime.UtcNow,
            IsDeleted = false,
        };

      

        await _bookRepository.AddAsync(newBook);
        return newBook;
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
