using HotChocolate.Types;
using Experion.PickMyBook.Infrastructure.Models;
using Experion.PickMyBook.Infrastructure.Models.DTO;

public class RequestType : ObjectType<Request>
{
    protected override void Configure(IObjectTypeDescriptor<Request> descriptor)
    {
        descriptor.Field(r => r.RequestType).Name("requestType");
    }
}

public class RequestDTOType : ObjectType<RequestDTO>
{
    protected override void Configure(IObjectTypeDescriptor<RequestDTO> descriptor)
    {
        descriptor.Field(r => r.RequestType).Name("requestType");
    }
}
public class UserRequestsType : ObjectType<UserRequestsDTO>
{
    protected override void Configure(IObjectTypeDescriptor<UserRequestsDTO> descriptor)
    {
        descriptor.Field(r => r.BookTitle).Name("bookTitle");
        descriptor.Field(r => r.RequestType).Name("requestType");
        descriptor.Field(r => r.RequestStatus).Name("requestStatus");
        descriptor.Field(r => r.Message).Name("message");
    }
}

