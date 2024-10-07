using Experion.PickMyBook.Infrastructure.Models.DTO;
using HotChocolate.Types;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Experion.PickMyBook.Business.Service.IService
{
    public interface IBookService
    {
        Task<IEnumerable<Book>> GetBooksAsync();
        Task<IEnumerable<Book>> GetAllBooksAsync();
        Task<Book> AddBookAsync(Book book);
        Task<Book> UpdateBookAsync(Book book);
        Task<int> GetTotalBooksCountAsync();
        Task<Book> UpdateBookStatusAsync(int bookId, bool isDeleted);

    }
}
