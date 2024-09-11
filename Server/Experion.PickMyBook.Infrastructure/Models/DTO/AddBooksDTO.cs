using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Experion.PickMyBook.Infrastructure.Models.DTO
{
    public class AddBooksDTO
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public string ISBN { get; set; }
        public string Publisher { get; set; }
        public int AvailableCopies { get; set; }
        public int PublishedYear { get; set; }
        public string Genre { get; set; }
        [MaxLength(3)]
        public string[]? ImageUrls { get; set; } = new string[3];

    }
}
