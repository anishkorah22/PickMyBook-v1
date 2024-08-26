using Experion.PickMyBook.Data;
using Experion.PickMyBook.Infrastructure.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Experion.PickMyBook.Business.Services
{
    public class BookService
    {
        private readonly IRepository<Book> _bookRepository;

        public BookService(IRepository<Book> bookRepository)
        {
            _bookRepository = bookRepository;
        }

        public async Task<IEnumerable<Book>> GetAllBooksAsync()
        {
            return await _bookRepository.GetAllAsync();
        }

        public async Task<Book> GetBookByIdAsync(int id)
        {
            return await _bookRepository.GetByIdAsync(id);
        }

        public async Task AddBookAsync(Book book)
        {
            await _bookRepository.AddAsync(book);
        }

        public async Task UpdateBookAsync(Book book)
        {
            await _bookRepository.UpdateAsync(book);
        }

        public async Task DeleteBookAsync(int id)
        {
            await _bookRepository.DeleteAsync(id);
        }

        public async Task<int> GetTotalBooksAsync()
        {
            var books = await _bookRepository.GetAllAsync();
            return books.Count(book => (bool)!book.IsDeleted);
        }
    }
}
