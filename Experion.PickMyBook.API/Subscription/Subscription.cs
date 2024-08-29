using HotChocolate;
using HotChocolate.Subscriptions;
using Experion.PickMyBook.Infrastructure.Models;

namespace Experion.PickMyBook.API.GraphQLTypes
{
    public class Subscription
    {
        [Subscribe]
        [Topic("OnBookStatusChanged")] 
        public Book OnBookStatusChanged([EventMessage] Book book)
        {
            return book;
        }

        [Subscribe]
        [Topic("OnUserStatusChanged")] 
        public User OnUserStatusChanged([EventMessage] User user)
        {
            return user;
        }
    }
}
