using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibraryManagementAPI.Core.Models;

namespace LibraryManagementAPI.Data.Repositories
{
    public class LibraryDbContext : DbContext
    {
        public LibraryDbContext()
        {

        }
        public LibraryDbContext(DbContextOptions<LibraryDbContext> options) : base(options)
        { }
        public virtual DbSet<Book> Books { get; set; }
        public virtual DbSet<Author> Authors { get; set; }
        public virtual DbSet<Member> Members { get; set; }
        public virtual DbSet<BorrowedBook> BorrowedBooks { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(
                      "Server=.;Database=LibraryManagementDb;Trusted_Connection=True;",
                    options => options.MigrationsAssembly("LibraryManagementAPI.Data")  // Burada doğru assemblyyi belirtiyoruz
                );
            }

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //book-authors
            modelBuilder.Entity<Book>()
                .HasOne(a => a.Authors)
                .WithMany(b=>b.Books)
                .HasForeignKey(f=>f.AuthorId)
                .OnDelete(DeleteBehavior.Cascade);


            modelBuilder.Entity<BorrowedBook>()
                .HasOne(b => b.Member)
                .WithMany(m => m.BorrowedBooks)
                .HasForeignKey(f => f.MemberId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<BorrowedBook>()
                .HasOne(b=>b.Books)
                .WithMany(x=> x.BorrowedBooks)
                .HasForeignKey(f=>f.BookId)
                .OnDelete(DeleteBehavior.Cascade);

        }

    }
}


