using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Experion.PickMyBook.Infrastructure.Models.DTO
{
    public class DashboardCountsDTO
    {
        public int TotalBooks { get; set; }
        public int TotalActiveUsers { get; set; }
        public int TotalCurrentBorrowTransactions { get; set; }
    }
}
