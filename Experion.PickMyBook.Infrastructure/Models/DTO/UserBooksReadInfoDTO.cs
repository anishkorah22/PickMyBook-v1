using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Experion.PickMyBook.Infrastructure.Models.DTO
{
    public class UserBooksReadInfoDTO
    {
        public int BooksReadCount { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public DateOnly BorrowDate { get; set; }
        public DateOnly ReturnDate { get; set; }
    }
}
