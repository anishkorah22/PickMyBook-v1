using Experion.PickMyBook.Data;
using HotChocolate.Types;

namespace Experion.PickMyBook.API.GraphQLTypes
{
    public class ApiMutationType : ObjectType<Mutation>
    {
        protected override void Configure(IObjectTypeDescriptor<Mutation> descriptor)
        {

            descriptor.Field(f => f.CreateUser(default!, default!, default!)).Type<UserType>();

            descriptor.Field(f => f.AddBook(default!, default!))
                .Type<BookType>();
            descriptor.Field(f => f.UpdateBook(default!, default!)).Type<BookType>();

            descriptor.Field(f => f.UpdateUser(default!, default!))
              .Type<UserType>()
              .Name("updateUser");

        }

    }
}