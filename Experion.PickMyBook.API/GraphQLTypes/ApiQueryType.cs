using HotChocolate.Types;
using static BorrowingType;
using static UserType;

namespace Experion.PickMyBook.API.GraphQLTypes
{
    public class ApiQueryType : ObjectType<Query>
    {
        protected override void Configure(IObjectTypeDescriptor<Query> descriptor)
        {


            // Existing fields
            descriptor.Field(f => f.GetBooks())
                .Type<ListType<BookType>>()
                .Name("books");

            descriptor.Field(f => f.GetAllUsers())
                .Type<ListType<UserType>>()
                .Name("users");

            descriptor.Field(f => f.GetUserById(default!))
                .Type<UserType>()
                .Name("user");

            descriptor.Field(f => f.GetBorrowings())
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
                .Type<ListType<RequestDTOType>>()
                .Name("requests");


            descriptor.Field(q => q.GetBooksReadByUser(default))
           .Argument("userId", a => a.Type<NonNullType<IntType>>())
           .Type<ListType<UserBooksReadInfoType>>()
           .Name("booksReadByUser");

            descriptor.Field(q => q.GetUserRequests(default))
               .Argument("userId", a => a.Type<NonNullType<IntType>>()) 
               .Type<ListType<UserRequestsType>>() 
               .Name("userRequests");

        }
    }
}