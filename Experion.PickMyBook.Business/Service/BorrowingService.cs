using Experion.PickMyBook.Data;
using Experion.PickMyBook.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Experion.PickMyBook.Business.Service
{
    public class BorrowingService
    {
        private readonly IRepository<Borrowings> _borrowingRepository;

        public BorrowingService(IRepository<Borrowings> borrowingRepository)
        {
            _borrowingRepository = borrowingRepository;
        }

        public async Task<int> GetTotalCurrentBorrowTransactionsAsync()
        {
            var borrowings = await _borrowingRepository.GetAllAsync();
            
            return borrowings.Count(borrowing => borrowing.Status == "Borrowed");
        }
    }
}
