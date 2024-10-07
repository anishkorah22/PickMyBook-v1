using Experion.PickMyBook.Infrastructure.Models;
using Experion.PickMyBook.Infrastructure.Models.DTO;

public class RequestType : ObjectType<Request>
{
    protected override void Configure(IObjectTypeDescriptor<Request> descriptor)
    {
        descriptor.Field(r => r.RequestType).Name("requestType").Type<StringType>();

        // If 'status' is part of the Request class, include it
        descriptor.Field(r => r.RequestTypeValue).Name("status").Type<StringType>();

        // Add other necessary fields as needed
        descriptor.Field(r => r.RequestId).Name("requestId").Type<IntType>();
        descriptor.Field(r => r.BookId).Name("bookId").Type<IntType>();
        descriptor.Field(r => r.UserId).Name("userId").Type<IntType>();
        descriptor.Field(r => r.RequestedAt).Name("requestedAt").Type<DateTimeType>();
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

