using HotChocolate;
using HotChocolate.Types;
using Experion.PickMyBook.Infrastructure.Models;
using Experion.PickMyBook.Data;
using Experion.PickMyBook.Business.Services;
using Experion.PickMyBook.Infrastructure;
using Experion.PickMyBook.Infrastructure.Models.DTO;

public class UserType : ObjectType<User>
{
    protected override void Configure(IObjectTypeDescriptor<User> descriptor)
    {
        descriptor.Field(u => u.Borrowings).ResolveWith<UserResolvers>(u => u.GetBorrowings(default!, default!));

    }

    private class UserResolvers
    {
        public IQueryable<Borrowings> GetBorrowings(User user, [Service] LibraryContext context)
        {
            return context.Borrowings.Where(b => b.UserId == user.UserId);
        }

    }
    public class DashboardCountsType : ObjectType<DashboardCountsDTO>
    {
        protected override void Configure(IObjectTypeDescriptor<DashboardCountsDTO> descriptor)
        {
            descriptor.Field(f => f.TotalBooks)
                .Type<IntType>()
                .Name("totalBooks");

            descriptor.Field(f => f.TotalActiveUsers)
                .Type<IntType>()
                .Name("totalActiveUsers");

            descriptor.Field(f => f.TotalCurrentBorrowTransactions)
                .Type<IntType>()
                .Name("totalCurrentBorrowTransactions");
        }
    }
}