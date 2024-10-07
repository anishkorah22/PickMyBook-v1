using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Experion.PickMyBook.Infrastructure.Models
{

    public class Request
    {
        [Key]
        public int RequestId { get; set; }

        [ForeignKey("Book")]
        public int BookId { get; set; }
        public Book? Book { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }

        public User? User { get; set; }

        [ForeignKey("RequestType")]
        public int RequestTypeValue { get; set; }

        public RequestType? RequestType { get; set; }

        public DateTime? RequestedAt { get; set; } = DateTime.UtcNow;

        [ForeignKey("RequestStatus")]
        public int RequestStatusValue { get; set; }
        public RequestStatus? RequestStatus { get; set; }  

        public string? Message { get; set; }
    }
}
