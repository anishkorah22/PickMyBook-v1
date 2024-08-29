using HotChocolate.Types;
using static UserType;

namespace Experion.PickMyBook.API.GraphQLTypes
{
    public class ApiQueryType : ObjectType<Query>
    {
        protected override void Configure(IObjectTypeDescriptor<Query> descriptor)
        {


            // Existing fields
            descriptor.Field(f => f.GetBooks(default!))
                .Type<ListType<BookType>>()
                .Name("books");

            descriptor.Field(f => f.GetAllUsers(default!))
                .Type<ListType<UserType>>()
                .Name("users");

            descriptor.Field(f => f.GetUserById(default!, default!))
                .Type<UserType>()
                .Name("user");

            descriptor.Field(f => f.GetBorrowings(default!))
                .Type<ListType<BorrowingType>>()
                .Name("borrowings");

            descriptor.Field(f => f.GetDashboardCountsAsync())
                            .Type<DashboardCountsType>()
                            .Name("dashboardCounts");

            descriptor.Field(q => q.GetBorrowingsByUserIdAsync(default))
                .Argument("userId", a => a.Type<NonNullType<IntType>>())
                .Type<ListType<BorrowingType>>()
                .Name("getBorrowingsByUserId");

            descriptor.Field(q => q.GetAllRequestsAsync())
                .Type<ListType<RequestType>>()
                .Name("requests");


        }
    }
}