using Experion.PickMyBook.Infrastructure.Models;
using Experion.PickMyBook.Business.Service.IService;

namespace Experion.PickMyBook.API.GraphQLTypes
{
    public class Subscription
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public Subscription(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        [Subscribe]
        [Topic("OnBookStatusChanged")]
        public async Task<Book> OnBookStatusChanged([EventMessage] Book book)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var bookService = scope.ServiceProvider.GetService<IBookService>();
            return book;
        }

        [Subscribe]
        [Topic("OnUserStatusChanged")]
        public async Task<User> OnUserStatusChanged([EventMessage] User user)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var userService = scope.ServiceProvider.GetService<IUserService>();
            return user;
        }
    }
}