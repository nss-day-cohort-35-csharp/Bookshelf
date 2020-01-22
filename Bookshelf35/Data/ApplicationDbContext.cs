using System;
using System.Collections.Generic;
using System.Text;
using Bookshelf35.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Bookshelf35.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Book> Book { get; set; }
        public DbSet<Author> Author { get; set; }
        public DbSet<Comment> Comment { get; set; }
        public DbSet<Genre> Genre { get; set; }
        public DbSet<BookGenre> BookGenre { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<BookGenre>()
                .HasOne(bc => bc.Book)
                .WithMany(b => b.BookGenres)
                .HasForeignKey(bc => bc.BookId);
            modelBuilder.Entity<BookGenre>()
                .HasOne(bc => bc.Genre)
                .WithMany(c => c.BookGenres)
                .HasForeignKey(bc => bc.GenreId);

            modelBuilder.Entity<Genre>().HasData(
                new Genre
                {
                    Id = 1,
                    Description = "Fantasy"
                },
                new Genre
                {
                    Id = 2,
                    Description = "Science Fiction"
                },
                new Genre
                {
                    Id = 3,
                    Description = "Horror"
                },
                new Genre
                {
                    Id = 4,
                    Description = "Western"
                },
                new Genre
                {
                    Id = 5,
                    Description = "Romance"
                },
                new Genre
                {
                    Id = 6,
                    Description = "Thriller"
                },
                new Genre
                {
                    Id = 7,
                    Description = "Mystery"
                },
                new Genre
                {
                    Id = 8,
                    Description = "Detective"
                },
                new Genre
                {
                    Id = 9,
                    Description = "Distopia"
                }
            );
        }
    }
}
