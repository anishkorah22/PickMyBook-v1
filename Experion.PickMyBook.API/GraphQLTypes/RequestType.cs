using HotChocolate.Types;
using Experion.PickMyBook.Infrastructure.Models;

public class RequestType : ObjectType<RequestDTO>
{
    protected override void Configure(IObjectTypeDescriptor<RequestDTO> descriptor)
    {
        descriptor.Field(r => r.BookTitle).Name("bookTitle");
        descriptor.Field(r => r.Username).Name("username");
        descriptor.Field(r => r.RequestType).Name("requestType");
    }
}
