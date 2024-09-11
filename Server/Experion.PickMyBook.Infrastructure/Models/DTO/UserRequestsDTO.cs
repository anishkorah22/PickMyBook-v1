using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Experion.PickMyBook.Infrastructure.Models.DTO
{
    public class UserRequestsDTO
    {
        public string BookTitle { get; set; }
        public string RequestType { get; set; }
        public string RequestStatus { get; set; } 
        public string Message { get; set; }
    }
}
