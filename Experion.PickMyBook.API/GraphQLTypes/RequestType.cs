using HotChocolate.Types;
using Experion.PickMyBook.Infrastructure.Models;

public class RequestType : ObjectType<Request>
{
    protected override void Configure(IObjectTypeDescriptor<Request> descriptor)
    {
        descriptor.Field(r => r.Book.Title).Name("bookTitle");
        descriptor.Field(r => r.User.UserName).Name("username");
        descriptor.Field(r => r.RequestType).Name("requestType");
        descriptor.Field(r => r.RequestedAt).Type<DateTimeType>();
        descriptor.Field(r => r.Status).Type<EnumType<RequestStatus>>();
        descriptor.Field(r => r.Message).Type<StringType>();
    }
}
