namespace Experion.PickMyBook.Infrastructure.Models
{
    public enum RequestStatusValue
    {
        Pending = 1,
        Approved = 2,
        Declined = 3
    }

    public class RequestStatus
    {
        public int RequestStatusId { get; set; }
        public RequestStatusValue Status { get; set; }
    }


}

