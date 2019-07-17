using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Bookstore.Models.DAL
{
    public class BookstoreContext : DbContext
    {
        public DbSet<Book> Books { get; set; }
        public DbSet<Author> Authors { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Book>()
                .HasKey(a => a.IBAN);

            modelBuilder.Entity<Book>()
                .Property(a => a.Title)
                .HasMaxLength(30);

            modelBuilder.Entity<Book>()
                .Property(a => a.Genre)
                .HasMaxLength(15);

            modelBuilder.Entity<Author>()
                .Property(a => a.Name)
                .HasMaxLength(30);
        }
    }
}