using Microsoft.EntityFrameworkCore;
using Experion.PickMyBook.Infrastructure.Models;

namespace Experion.PickMyBook.Data
{
    public class LibraryContext : DbContext
    {
        public LibraryContext(DbContextOptions<LibraryContext> options) : base(options) { }

        public DbSet<Book> Books { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Borrowings> Borrowings { get; set; }

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
        }
    }
}
