using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library.API.Entities
{
    public static class ModelBuilderExtension
    {
        public static void SeedData(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Author>().HasData(new Author
                {
                    Id = new Guid("72D5B5F5-3008-49B7-B0D6-CC337F1A3330"),
                    Name = "Author 1",
                    BirthDate = new DateTimeOffset(new DateTime(1996, 11, 18)),
                    Email = "author1@xxx.com",
                    BirthPlace = "AAAA"
            });
            modelBuilder.Entity<Book>().HasData(new Book
            { 
                Id = Guid.NewGuid(),
                Pages = 200,
                Title = "Book 1",
                AuthorId = new Guid("72D5B5F5-3008-49B7-B0D6-CC337F1A3330"),

            });
        }
    }
}
