using HotChocolate;
using HotChocolate.Types;
using Experion.PickMyBook.Infrastructure.Models;
using Experion.PickMyBook.Data;
using Experion.PickMyBook.Business.Services;
using Experion.PickMyBook.Infrastructure;

public class UserType : ObjectType<User>
{
    protected override void Configure(IObjectTypeDescriptor<User> descriptor)
    {
        descriptor.Field(u => u.Borrowings).ResolveWith<UserResolvers>(u => u.GetBorrowings(default!, default!));
       /* descriptor.Field("totalActiveUsers").ResolveWith<UserResolvers>(u => u.GetTotalActiveUsers(default!));*/
    }

    private class UserResolvers
    {
        public IQueryable<Borrowings> GetBorrowings(User user, [Service] LibraryContext context)
        {
            return context.Borrowings.Where(b => b.UserId == user.UserId);
        }

        /*public async Task<int> GetTotalActiveUsers([Service] UserService userService)
        {
            return await userService.GetTotalActiveUsersAsync();
        }*/
    }
}
