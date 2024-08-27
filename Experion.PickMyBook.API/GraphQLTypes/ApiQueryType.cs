using HotChocolate.Types;

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
        }
    }
}
