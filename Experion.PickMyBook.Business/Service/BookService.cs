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
    public async Task<Book> AddBookAsync(AddBooksDTO dto, IEnumerable<IFile>? files)
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
            CreatedAt = DateTime.UtcNow
        };

        if (files != null && files.Any())
        {
            var imageUrls = new List<string>();
            var uploadPath = Path.Combine(_environment.WebRootPath, "uploads");
            Console.WriteLine($"WebRootPath: {_environment.WebRootPath}");


            if (!Directory.Exists(uploadPath))
            {
                Directory.CreateDirectory(uploadPath);
            }

            foreach (var file in files)
            {
                var filePath = Path.Combine(uploadPath, file.Name);

                try
                {
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }

                    var url = $"/uploads/{file.Name}";
                    imageUrls.Add(url);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error uploading file {file.Name}: {ex.Message}");
                }
            }
            book.ImageUrls = imageUrls.ToArray();
        }

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
