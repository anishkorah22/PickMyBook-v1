using Experion.PickMyBook.Data;
using HotChocolate.Types;

namespace Experion.PickMyBook.API.GraphQLTypes
{
    public class ApiQueryType : ObjectType<Query>
    {
        protected override void Configure(IObjectTypeDescriptor<Query> descriptor)
        {

            descriptor.Field(f => f.GetUserByUsername(default!, default!)).Type<UserType>();

        }
    }

}