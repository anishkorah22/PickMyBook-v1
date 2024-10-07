using Experion.PickMyBook.Infrastructure.Models;

namespace Experion.PickMyBook.API.GraphQLTypes
{
    public class RoleTypeValueEnumType : EnumType<RoleTypeValue>
    {
        protected override void Configure(IEnumTypeDescriptor<RoleTypeValue> descriptor)
        {
            descriptor.Name("RoleTypeValue");
            descriptor.Value(RoleTypeValue.Admin).Name("ADMIN");
            descriptor.Value(RoleTypeValue.Staff).Name("STAFF");
            descriptor.Value(RoleTypeValue.User).Name("USER");
        }
    }

}
