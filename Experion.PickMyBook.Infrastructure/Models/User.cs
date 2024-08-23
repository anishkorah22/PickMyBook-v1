﻿using System;
using System.Collections.Generic;

namespace Experion.PickMyBook.Infrastructure.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public IEnumerable<string> Roles { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public ICollection<Borrowings> Borrowings { get; set; }

        
    }
}
