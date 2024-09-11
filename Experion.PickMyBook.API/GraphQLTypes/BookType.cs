﻿using HotChocolate;
using HotChocolate.Types;
using Experion.PickMyBook.Infrastructure.Models;
using Experion.PickMyBook.Data;
using Experion.PickMyBook.Business.Services;
using Experion.PickMyBook.Infrastructure;

public class BookType : ObjectType<Book>
{
    protected override void Configure(IObjectTypeDescriptor<Book> descriptor)
    {
        descriptor.Field(b => b.Borrowings).ResolveWith<BookResolvers>(b => b.GetBorrowings(default!, default!));
        /*descriptor.Field("totalBooks").ResolveWith<BookResolvers>(b => b.GetTotalBooks(default!));*/
    }

    private class BookResolvers
    {
        public IQueryable<Borrowings> GetBorrowings(Book book, [Service] LibraryContext context)
        {
            return context.Borrowings.Where(b => b.BookId == book.BookId);
        }


    }
}
