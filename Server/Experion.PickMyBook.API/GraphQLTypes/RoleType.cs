using Experion.PickMyBook.Infrastructure.Models;

namespace Experion.PickMyBook.API.GraphQLTypes
{
    public class RoleType : ObjectType<Role>
    {
        protected override void Configure(IObjectTypeDescriptor<Role> descriptor)
        {
            descriptor.Field(r => r.RoleTypeId).Type<NonNullType<IdType>>();
            descriptor.Field(r => r.RoleTypeValue).Type<NonNullType<StringType>>();
        }
    }
}
