using Experion.PickMyBook.Infrastructure.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Experion.PickMyBook.Business.Service.IService
{
    public interface IBookService
    {
        Task<Book> AddBookAsync(AddBooksDTO dto);
        Task<Book> UpdateBookAsync(Book book);
    }
}
