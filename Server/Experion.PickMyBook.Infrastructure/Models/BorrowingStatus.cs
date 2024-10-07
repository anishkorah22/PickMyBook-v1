namespace Experion.PickMyBook.Infrastructure.Models
{
    public enum BorrowingStatusValue
    {
        Borrowed =1,
        Returned =2,
        
    }

    public class BorrowingStatus
    {
        public int BorrowingStatusId { get; set; }
        public BorrowingStatusValue Status { get; set; }
    }
}
