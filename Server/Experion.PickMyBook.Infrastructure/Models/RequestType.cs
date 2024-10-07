namespace Experion.PickMyBook.Infrastructure.Models
{
    public enum RequestTypeValue
    {
        BorrowRequest = 1,
        ReturnRequest = 2
    }

    public class RequestType
    {
        public int RequestTypeId { get; set; }
        public RequestTypeValue Type { get; set; }
    }
}
