using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Experion.PickMyBook.Infrastructure.Models
{
    public enum RequestType
    {
        BorrowRequest,
        ReturnRequest
    }

    public enum RequestStatus
    {
        Pending,
        Approved,
        Declined
    }

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

        public RequestType RequestType { get; set; }

        public DateTime? RequestedAt { get; set; } = DateTime.UtcNow;

        public RequestStatus Status { get; set; } = RequestStatus.Pending;  // Enum: Pending, Approved, Declined

        public string? Message { get; set; }
    }
}
