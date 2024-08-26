using HotChocolate.Types;

namespace Experion.PickMyBook.API.GraphQLTypes
{
    public class ApiQueryType : ObjectType<Query>
    {
        protected override void Configure(IObjectTypeDescriptor<Query> descriptor)
        {
            /*descriptor.Field(f => f.GetBookCountAsync())
                .Type<IntType>()
                .Name("bookCount");

            descriptor.Field(f => f.GetBorrowingCountAsync())
                .Type<IntType>()
                .Name("borrowingCount");

            descriptor.Field(f => f.GetUserCountAsync())
                .Type<IntType>()
                .Name("userCount");*/

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
        }
    }
}
