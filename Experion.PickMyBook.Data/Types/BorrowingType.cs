using HotChocolate;
using HotChocolate.Types;
using Experion.PickMyBook.Infrastructure.Models;
using Experion.PickMyBook.Data;

public class BorrowingType : ObjectType<Borrowings>
{
    protected override void Configure(IObjectTypeDescriptor<Borrowings> descriptor)
    {
        descriptor.Field(b => b.User).ResolveWith<BorrowingResolvers>(b => b.GetUser(default!, default!));
        descriptor.Field(b => b.Book).ResolveWith<BorrowingResolvers>(b => b.GetBook(default!, default!));
    }

    private class BorrowingResolvers
    {
        public User GetUser(Borrowings borrowing, [Service] LibraryContext context)
        {
            return context.Users.FirstOrDefault(u => u.Id == borrowing.UserId);
        }

        public Book GetBook(Borrowings borrowing, [Service] LibraryContext context)
        {
            return context.Books.FirstOrDefault(b => b.Id == borrowing.BookId);
        }
    }
}
