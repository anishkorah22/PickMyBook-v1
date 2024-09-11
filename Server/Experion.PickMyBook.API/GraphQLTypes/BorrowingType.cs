using HotChocolate;
using HotChocolate.Types;
using Experion.PickMyBook.Infrastructure.Models;
using Experion.PickMyBook.Data;
using Experion.PickMyBook.Business.Services;
using Experion.PickMyBook.Business.Service;
using Experion.PickMyBook.Infrastructure;
using Experion.PickMyBook.Infrastructure.Models.DTO;

public class BorrowingType : ObjectType<Borrowings>
{
    protected override void Configure(IObjectTypeDescriptor<Borrowings> descriptor)
    {
        descriptor.Field(b => b.BorrowDate);
        descriptor.Field(b => b.ReturnDate);
        descriptor.Field(b => b.Book)
                 .Type<BookType>(); // or resolve it with a resolver

        descriptor.Field(b => b.User)
                  .Type<UserType>();

    }

    private class BorrowingResolvers
    {
        public User GetUser(Borrowings borrowing, [Service] LibraryContext context)
        {
            return context.Users.FirstOrDefault(u => u.UserId == borrowing.UserId);
        }

        public Book GetBook(Borrowings borrowing, [Service] LibraryContext context)
        {
            return context.Books.FirstOrDefault(b => b.BookId == borrowing.BookId);
        }

    }

    public class UserBooksReadInfoType : ObjectType<UserBooksReadInfoDTO>
    {
        protected override void Configure(IObjectTypeDescriptor<UserBooksReadInfoDTO> descriptor)
        {
            descriptor.Field(b => b.BooksReadCount).Type<NonNullType<IntType>>();
            descriptor.Field(b => b.Title).Type<NonNullType<StringType>>();
            descriptor.Field(b => b.Author).Type<NonNullType<StringType>>();
            descriptor.Field(b => b.BorrowDate).Type<NonNullType<DateTimeType>>();
            descriptor.Field(b => b.ReturnDate).Type<NonNullType<DateTimeType>>();
        }
    }
    
}
