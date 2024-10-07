using System.ComponentModel.DataAnnotations;

namespace Experion.PickMyBook.Infrastructure.Models
{
    public class Role
    {
        [Key]
        public int RoleTypeId { get; set; }
        public string RoleTypeValue { get; set; }
    }
    public enum RoleTypeValue
    {
        Admin = 1,
        Staff = 2,
        User= 3
    }

}
