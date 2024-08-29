﻿using Experion.PickMyBook.Data;
using HotChocolate.Types;

namespace Experion.PickMyBook.API.GraphQLTypes
{
    public class ApiMutationType : ObjectType<Mutation>
    {
        protected override void Configure(IObjectTypeDescriptor<Mutation> descriptor)
        {
            descriptor.Field(f => f.CreateUser(default!, default!, default!))
                .Type<UserType>();

            descriptor.Field(f => f.AddBook(default!, default!))
                .Type<BookType>();

            descriptor.Field(f => f.UpdateBook(default!, default!))
                .Type<BookType>();

            descriptor.Field(m => m.BorrowBook(default!, default!, default!))
                .Type<BorrowingType>()
                .Argument("bookId", a => a.Type<NonNullType<IntType>>())
                .Argument("userId", a => a.Type<NonNullType<IntType>>());

            descriptor.Field(m => m.UpdateBorrowing(default!, default!))
                .Type<BorrowingType>();

            descriptor.Field(f => f.UpdateUser(default!, default!))
              .Type<UserType>()
              .Name("updateUser");

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


        }
    }
}
