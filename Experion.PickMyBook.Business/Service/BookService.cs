﻿using Experion.PickMyBook.Infrastructure.Models.DTO;
using Experion.PickMyBook.Infrastructure;
using Experion.PickMyBook.Data;
using Experion.PickMyBook.Business.Service.IService;
using Microsoft.EntityFrameworkCore;


public class BookService : IBookService
{
    private readonly LibraryContext _context;
    private readonly BookRepository _bookRepository;
    public BookService(LibraryContext context)
    {
        _context = context;
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
        };

        _context.Books.Add(book);
        await _context.SaveChangesAsync();
        return book;
    }

    public async Task<Book> UpdateBookAsync(Book book)
    {
        var existingBook = await _context.Books.FindAsync(book.BookId);

        if (existingBook == null)
        {
            throw new KeyNotFoundException("Book not found.");
        }

        existingBook.Title = !string.IsNullOrEmpty(book.Title) ? book.Title : existingBook.Title;
        existingBook.Author = !string.IsNullOrEmpty(book.Author) ? book.Author : existingBook.Author;
        existingBook.ISBN = !string.IsNullOrEmpty(book.ISBN) ? book.ISBN : existingBook.ISBN;
        existingBook.Publisher = !string.IsNullOrEmpty(book.Publisher) ? book.Publisher : existingBook.Publisher;
        existingBook.AvailableCopies = book.AvailableCopies != 0 || existingBook.AvailableCopies == 0 ? book.AvailableCopies : existingBook.AvailableCopies;
        existingBook.PublishedYear = book.PublishedYear != 0 || existingBook.PublishedYear == 0 ? book.PublishedYear : existingBook.PublishedYear;
        existingBook.Genre = !string.IsNullOrEmpty(book.Genre) ? book.Genre : existingBook.Genre;

        existingBook.UpdatedAt = DateTime.UtcNow;

        _context.Books.Update(existingBook);
        await _context.SaveChangesAsync();
        return existingBook;

        
    }
    public async Task DeleteBookAsync(int id)
    {
        await _bookRepository.DeleteAsync(id);
    }

    public async Task<int> GetTotalBooksCountAsync()
    {
        return (int)await _context.Books
            .Where(b => (b.IsDeleted == false || !b.IsDeleted.HasValue) && b.AvailableCopies > 0)
            .SumAsync(b => b.AvailableCopies);
    }





}
