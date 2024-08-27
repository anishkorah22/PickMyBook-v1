using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Experion.PickMyBook.Business.Service.IService
{
    public interface IUserService
    {
        Task<int> GetTotalActiveUsersCountAsync();

    }
}
