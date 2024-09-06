using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Experion.PickMyBook.Infrastructure.Models;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Collections.Generic;
using System.Linq;

namespace Experion.PickMyBook.Infrastructure
{
    public class LibraryContext : DbContext
    {
        public LibraryContext(DbContextOptions<LibraryContext> options) : base(options) { }

        public DbSet<Book> Books { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Borrowings> Borrowings { get; set; }
        public DbSet<Request> Requests { get; set; }

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
                .Property(u => u.Roles)
                .HasConversion(
                    v => string.Join(',', v),
                    v => v.Split(',', StringSplitOptions.RemoveEmptyEntries))
                .Metadata
                .SetValueComparer(new ValueComparer<IEnumerable<string>>(
                    (c1, c2) => c1.SequenceEqual(c2),
                    c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                    c => c.ToList()));

            modelBuilder.Entity<Borrowings>()
                .HasKey(b => new { b.BookId, b.UserId });
/*
            modelBuilder.Entity<Request>()
               .HasKey(b => new { b.BookId, b.UserId });*/


            modelBuilder.Entity<Request>()
                .HasOne(r => r.Book)
                .WithMany()
                .HasForeignKey(r => r.BookId);

            modelBuilder.Entity<Request>()
                .HasOne(r => r.User)
                .WithMany()
                .HasForeignKey(r => r.UserId);

            modelBuilder.Entity<Request>()
                .Property(r => r.RequestedAt)
                .HasDefaultValueSql("NOW() AT TIME ZONE 'UTC'");  
        }
    }
}
