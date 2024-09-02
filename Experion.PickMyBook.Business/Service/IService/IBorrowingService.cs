using Experion.PickMyBook.Infrastructure.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Experion.PickMyBook.Business.Service.IService
{
    public interface IBorrowingService
    {
        Task<Borrowings> BorrowBookAsync(int userId, int bookId);
        Task<Borrowings> UpdateBorrowingAsync(Borrowings borrowing);
        Task<Borrowings> ReturnBookAsync(int userId, int bookId);
        Task<int> GetTotalBorrowingsCountAsync();
        Task<List<Borrowings>> GetBorrowingsByUserIdAsync(int userId);
        Task<List<UserBooksReadInfoDTO>> GetBooksReadByUserAsync(int userId);
    }
}
