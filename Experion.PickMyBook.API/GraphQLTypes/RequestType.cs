using HotChocolate.Types;
using Experion.PickMyBook.Infrastructure.Models;

public class RequestType : ObjectType<RequestDTO>
{
    protected override void Configure(IObjectTypeDescriptor<RequestDTO> descriptor)
    {
        descriptor.Field(r => r.BookTitle).Name("bookTitle");
        descriptor.Field(r => r.Username).Name("username");
        descriptor.Field(r => r.RequestType).Name("requestType");
        descriptor.Field(r => r.RequestedAt).Type<DateTimeType>();
        descriptor.Field(r => r.Status).Type<EnumType<RequestStatus>>();
        descriptor.Field(r => r.Message).Type<StringType>();
    }
}
