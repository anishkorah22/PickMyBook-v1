using Experion.PickMyBook.Infrastructure.Models;
using Experion.PickMyBook.Infrastructure;
using Experion.PickMyBook.Infrastructure.Models.DTO;
using Experion.PickMyBook.API.GraphQLTypes;
using HotChocolate.Types.Relay;

public class UserType : ObjectType<User>
{
    protected override void Configure(IObjectTypeDescriptor<User> descriptor)
    {
        descriptor.Field(u => u.UserId).Type<NonNullType<IntType>>();
        descriptor.Field(u => u.UserName).Type<NonNullType<StringType>>();
        descriptor.Field(u => u.Role).Type<RoleType>();
        descriptor.Field(u => u.IsDeleted).Type<NonNullType<BooleanType>>();
        descriptor.Field(u => u.CreatedAt).Type<NonNullType<DateTimeType>>();
        descriptor.Field(u => u.UpdatedAt).Type<NonNullType<DateTimeType>>();
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