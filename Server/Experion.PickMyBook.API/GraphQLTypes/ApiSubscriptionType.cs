namespace Experion.PickMyBook.API.GraphQLTypes
{
    public class ApiSubscriptionType : ObjectType<Subscription>
    {
        protected override void Configure(IObjectTypeDescriptor<Subscription> descriptor)
        {
            descriptor.Field(t => t.OnBookStatusChanged(default!))
                .Type<BookType>()
                .Name("bookStatusChanged");

            descriptor.Field(t => t.OnUserStatusChanged(default!))
               .Type<UserType>()
               .Name("userStatusChanged");
        }
    }
}
