using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Bookstore.Models
{
    public class BookstoreContext : DbContext
    {
        public BookstoreContext() : base("name=BookstoreContext")
        {
        }

        public DbSet<Book> Books { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Genre> Genres { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Book>()
                .HasKey(x => x.ISBN);

            modelBuilder.Entity<Book>()
                .Property(x => x.ISBN)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None)
                .IsRequired();

            modelBuilder.Entity<Book>()
                .HasRequired(x => x.Author);

            modelBuilder.Entity<Book>()
                .Property(x => x.Description)
                .IsRequired();

            modelBuilder.Entity<Book>()
                .Property(x => x.Publisher)
                .IsRequired();

            modelBuilder.Entity<Book>()
                .Property(x => x.Format)
                .IsRequired();

            modelBuilder.Entity<Book>()
                .HasRequired(x => x.Genre);

            modelBuilder.Entity<Author>()
                .HasKey(x => x.Name);

            modelBuilder.Entity<Author>()
                .Property(x => x.Name)
                .HasMaxLength(50)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            modelBuilder.Entity<Genre>()
                .HasKey(x => x.Name);

            modelBuilder.Entity<Genre>()
                .Property(x => x.Name)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
        }
    }
}