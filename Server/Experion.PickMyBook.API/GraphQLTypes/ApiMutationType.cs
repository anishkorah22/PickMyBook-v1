using Experion.PickMyBook.Infrastructure.Models;
using Experion.PickMyBook.Infrastructure.Models.DTO;

namespace Experion.PickMyBook.API.GraphQLTypes
{
    public class ApiMutationType : ObjectType<Mutation>
    {
        protected override void Configure(IObjectTypeDescriptor<Mutation> descriptor)
        {
            descriptor.Field(f => f.CreateUser(default!, default))
            .Type<UserType>()
            .Argument("userName", a => a.Type<NonNullType<StringType>>())
            .Argument("roleTypeValue", a => a.Type<NonNullType<RoleTypeValueEnumType>>());


            descriptor.Field(f => f.AddBook(default!))
                .Type<BookType>()
                .Argument("book", a => a.Type<NonNullType<InputObjectType<Book>>>());


            descriptor.Field(f => f.UpdateBook(default!))
                .Type<BookType>();

            descriptor.Field(m => m.BorrowBook(default!, default!))
                .Type<BorrowingType>()
                .Argument("bookId", a => a.Type<NonNullType<IntType>>())
                .Argument("userId", a => a.Type<NonNullType<IntType>>());

            descriptor.Field(m => m.UpdateBorrowing(default!))
                .Type<BorrowingType>();

            descriptor.Field(f => f.UpdateUser(default!, default!, default))
            .Type<UserType>()
            .Name("updateUser")
            .Argument("userId", a => a.Type<NonNullType<IntType>>())
            .Argument("userName", a => a.Type<NonNullType<StringType>>())
            .Argument("roleTypeValue", a => a.Type<NonNullType<RoleTypeValueEnumType>>());

            descriptor.Field(f => f.UpdateBookStatusAsync(default!, default!))
                .Type<BookType>()
                .Name("updateBookStatus") 
                .Argument("bookId", a => a.Type<NonNullType<IntType>>())
                .Argument("isDeleted", a => a.Type<BooleanType>());

            descriptor.Field(f => f.UpdateUserStatusAsync(default!, default!))
                .Type<UserType>()
                .Name("updateUserStatus")
                .Argument("userId", a => a.Type<NonNullType<IntType>>())
                .Argument("isDeleted", a => a.Type<BooleanType>());

            descriptor.Field(m => m.CreateBorrowRequestAsync(default!, default!))
                 .Type<RequestType>()
                 .Argument("bookId", a => a.Type<NonNullType<IntType>>())
                 .Argument("userId", a => a.Type<NonNullType<IntType>>());

            descriptor.Field(m => m.CreateReturnRequestAsync(default!, default!))
                .Type<RequestType>()
                .Name("createReturnRequestAsync")
                .Argument("bookId", a => a.Type<NonNullType<IntType>>())
                .Argument("userId", a => a.Type<NonNullType<IntType>>());

            descriptor.Field(m => m.ApproveRequestAsync(default!))
                .Type<RequestType>()
                .Argument("requestId", a => a.Type<NonNullType<IntType>>())
                .Name("approveRequest");

            descriptor.Field(m => m.DeclineRequestAsync(default!,default!))
                .Type<RequestType>()
                .Argument("requestId", a => a.Type<NonNullType<IntType>>())
                .Argument("message", a=> a.Type<NonNullType<StringType>>())
                .Name("declineRequest");

        }
    }
}
