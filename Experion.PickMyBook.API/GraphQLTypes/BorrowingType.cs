using HotChocolate;
using HotChocolate.Types;
using Experion.PickMyBook.Infrastructure.Models;
using Experion.PickMyBook.Data;
using Experion.PickMyBook.Business.Services;
using Experion.PickMyBook.Business.Service;
using Experion.PickMyBook.Infrastructure;

public class BorrowingType : ObjectType<Borrowings>
{
    protected override void Configure(IObjectTypeDescriptor<Borrowings> descriptor)
    {
        descriptor.Field(b => b.User).ResolveWith<BorrowingResolvers>(b => b.GetUser(default!, default!));
        descriptor.Field(b => b.Book).ResolveWith<BorrowingResolvers>(b => b.GetBook(default!, default!));
        /*descriptor.Field("totalCurrentBorrowTransactions").ResolveWith<BorrowingResolvers>(b => b.GetTotalCurrentBorrowTransactions(default!));*/
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

      /*  public async Task<int> GetTotalCurrentBorrowTransactions([Service] BorrowingService borrowingService)
        {
            return await borrowingService.GetTotalCurrentBorrowTransactionsAsync();
        }*/
    }
}
