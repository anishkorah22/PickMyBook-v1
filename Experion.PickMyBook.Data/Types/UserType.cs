using Experion.PickMyBook.Infrastructure.Models;
using Experion.PickMyBook.Data;

public class UserType : ObjectType<User>
{
    protected override void Configure(IObjectTypeDescriptor<User> descriptor)
    {
        descriptor.Field(u => u.UserId);
        descriptor.Field(u => u.UserName);
        descriptor.Field(u => u.Roles);
        descriptor.Field(u => u.IsDeleted);
        descriptor.Field(u => u.CreatedAt);
        descriptor.Field(u => u.UpdatedAt);
        descriptor.Field(u => u.Borrowings).ResolveWith<UserResolvers>(u => u.GetBorrowings(default!, default!));   
    }

    private class UserResolvers
    {
        public IQueryable<Borrowings> GetBorrowings(User user, [Service] LibraryContext context)
        {
            return context.Borrowings.Where(b => b.UserId == user.UserId);
        }
    }
}