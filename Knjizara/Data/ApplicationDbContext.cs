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
        public DbSet<BookUserBorrow> BookUserBorrowTransaction { get; set; }
        public DbSet<BookUserBuy> BookUserBuyTransaction { get; set; }
        public DbSet<UserReturnBorrowedBook> UserReturnBorrowedBookTransaction { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Author>()
             .HasMany(c => c.Books)
            .WithOne(e => e.Author);

            modelBuilder.Entity<Book>().HasOne(e => e.Author).
                WithMany(a=>a.Books).HasForeignKey(b=>b.AuthorId);


            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<AppUser>().Property<bool>("isDeleted");
            modelBuilder.Entity<AppUser>().HasQueryFilter(u => EF.Property<bool>(u, "isDeleted") == false);

        }

        public override int SaveChanges()
        {
            UpdateSoftDeleteStatus();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            UpdateSoftDeleteStatus();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        private void UpdateSoftDeleteStatus()
        {
            foreach (var entry in ChangeTracker.Entries())
            {
                switch(entry.State)
                {
                    case EntityState.Added:
                        entry.CurrentValues["isDeleted"] = false;
                        break;
                    case EntityState.Deleted:
                        entry.State = EntityState.Modified;
                        entry.CurrentValues["isDeleted"] = true;
                        break;
                }
            }
        }
    }
}