
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Experion.PickMyBook.Infrastructure.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }
        public string UserName { get; set; }

        [ForeignKey("Role")]
        public int RoleTypeId { get; set; }
        public Role Role { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public ICollection<Borrowings>? Borrowings { get; set; }

        
    }
  
}
