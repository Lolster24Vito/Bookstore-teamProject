using Knjizara.Models.Authentication;
using Knjizara.Models.BaseEntities;
using Knjizara.Models.Books;
using Knjizara.Models.Transactions;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Knjizara.Data
{
    public class ApplicationDbContext : IdentityDbContext<AppUser, AppRole, Guid>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Book> Books { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<AppUser> AppUser { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
           modelBuilder.Entity<Author>()
             .HasMany(c => c.Books)
            .WithOne(e => e.Author);

            modelBuilder.Entity<Book>().HasOne(e => e.Author).
                WithMany(a=>a.Books).HasForeignKey(b=>b.AuthorId);

           

            modelBuilder.Entity<Book>().HasMany(b => b.UsersBorrowed).WithMany(u => u.BorrowedBooks);
            modelBuilder.Entity<Book>().HasMany(b => b.UsersPurchased).WithMany(u => u.PurchasedBooks);




            /*    
             *    
             *                modelBuilder.Entity<BookUserTransaction>().HasKey(x => new { x.PurchasedBookId, x.UserId, x.BorrowedBookId });

            modelBuilder.Entity<BookUserTransaction>().HasOne(x => x.User).WithMany(u => u.BorrowedBooks).HasForeignKey(k => k.BorrowedBookId);
            modelBuilder.Entity<BookUserTransaction>().HasMany(x=>x.BorrowedBook).
             *    
             *    
             *    modelBuilder.Entity<BookUserTransaction>().HasKey(but => new { but.UserId, but.BorrowedBookId });
                        modelBuilder.Entity<BookUserTransaction>().HasKey(but => new { but.UserId, but.PurchasedBookId });

                        modelBuilder.Entity<BookUserTransaction>().HasOne(bu=>bu.User).WithMany(u=>u.BorrowedBooks).HasForeignKey(bu=>bu.BorrowedBookId);
                      //  modelBuilder.Entity<BookUserTransaction>().HasOne(bu => bu.User).WithMany(u => u.BorrowedBooks).HasForeignKey(bu => bu.BorrowedBookId);


                        modelBuilder.Entity<BookUserTransaction>().HasOne(bu => bu.User).WithMany(u => u.PurchasedBooks).HasForeignKey(bu => bu.PurchasedBookId);
                       // modelBuilder.Entity<BookUserTransaction>().HasOne(bu => bu.User).WithMany(u => u.PurchasedBooks).HasForeignKey(bu => bu.PurchasedBookId);
            */

            // modelBuilder.Entity<Book>().HasMany(b=>b.UsersBorrowed).WithMany(x=>x.BorrowedBook)
            //    modelBuilder.Entity<Book>().HasMany(b => b.UsersPurchased).WithMany(u => u.PurchasedBooks);
            //      modelBuilder.Entity<Book>().HasMany(b => b.UsersBorrowed).WithMany(u => u.BorrowedBooks);

            /*
                        modelBuilder.Entity<BookUserTransaction>()
                        .HasKey(t => new { t.BookId, t.UserId });

                        modelBuilder.Entity<BookUserTransaction>().HasOne(bu => bu.Book).WithMany(b=>b.UsersBorrowed).HasForeignKey(bu=>bu.UserId);

                        modelBuilder.Entity<BookUserTransaction>().HasOne(bu => bu.Book).WithMany(b => b.UsersPurchased).HasForeignKey(bu => bu.UserId);
            */





            base.OnModelCreating(modelBuilder);

        }
    }
}