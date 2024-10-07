using Microsoft.EntityFrameworkCore;
using Experion.PickMyBook.Infrastructure.Models;

namespace Experion.PickMyBook.Infrastructure
{
    public class LibraryContext : DbContext
    {
        public LibraryContext(DbContextOptions<LibraryContext> options) : base(options) { }

        public DbSet<Book> Books { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Borrowings> Borrowings { get; set; }
        public DbSet<Request> Requests { get; set; }
        public DbSet<RequestType> RequestTypes { get; set; }
        public DbSet<RequestStatus> RequestStatuses { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<BorrowingStatus> BorrowingStatuses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Book>()
                .HasMany(b => b.Borrowings)
                .WithOne(b => b.Book)
                .HasForeignKey(b => b.BookId);

            modelBuilder.Entity<User>()
                .HasMany(u => u.Borrowings)
                .WithOne(b => b.User)
                .HasForeignKey(b => b.UserId);

            modelBuilder.Entity<User>()
                .HasOne(u => u.Role)
                .WithMany()
                .HasForeignKey(u => u.RoleTypeId);

            modelBuilder.Entity<Borrowings>()
                .HasKey(b => new { b.BookId, b.UserId });

            modelBuilder.Entity<Borrowings>()
                .HasOne(b => b.BorrowingStatus)
                .WithMany()
                .HasForeignKey(b => b.BorrowingStatusValue);

            modelBuilder.Entity<Request>()
                .HasOne(r => r.Book)
                .WithMany()
                .HasForeignKey(r => r.BookId);

            modelBuilder.Entity<Request>()
                .HasOne(r => r.User)
                .WithMany()
                .HasForeignKey(r => r.UserId);

            modelBuilder.Entity<Request>()
                .HasOne(r => r.RequestStatus)
                .WithMany()
                .HasForeignKey(r => r.RequestStatusValue);

            modelBuilder.Entity<Request>()
                .HasOne(r => r.RequestType)
                .WithMany()
                .HasForeignKey(r => r.RequestTypeValue);

            modelBuilder.Entity<RequestType>()
                .HasData(
                    new RequestType { RequestTypeId = 1, Type = RequestTypeValue.BorrowRequest },
                    new RequestType { RequestTypeId = 2, Type = RequestTypeValue.ReturnRequest }
                );

            modelBuilder.Entity<RequestStatus>()
                .HasData(
                    new RequestStatus { RequestStatusId = 1, Status = RequestStatusValue.Pending },
                    new RequestStatus { RequestStatusId = 2, Status = RequestStatusValue.Approved },
                    new RequestStatus { RequestStatusId = 3, Status = RequestStatusValue.Declined }
                );

            modelBuilder.Entity<BorrowingStatus>()
                .HasData(
                    new BorrowingStatus { BorrowingStatusId = 1, Status = BorrowingStatusValue.Borrowed },
                    new BorrowingStatus { BorrowingStatusId = 2, Status = BorrowingStatusValue.Returned }
                   
                );

            modelBuilder.Entity<Request>()
                .Property(r => r.RequestedAt)
                .HasDefaultValueSql("NOW() AT TIME ZONE 'UTC'");
        }
    }
}
