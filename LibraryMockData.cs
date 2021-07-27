using Library.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library.API
{
    public class LibraryMockData
    {
        public static LibraryMockData Current { get; } = new LibraryMockData();
        public List<AuthorDto> Authors { get; set; }
        public List<BookDto> Books { get; set; }

        public LibraryMockData()
        {
            Authors = new List<AuthorDto>()
            {
                new AuthorDto{Id = new Guid("00000000-0000-0000-0000-000000000000"), Name = "Author 1", Age = 46, Email = "author1@xxx.com"},
                new AuthorDto{Id = new Guid("00000000-0000-0000-0000-000000000001"), Name = "Author 2", Age = 38, Email = "author2@xxx.com"}
            };

            Books = new List<BookDto>()
            {
                new BookDto
                {
                    Id = new Guid("00000000-0000-0000-0000-000000000010"),
                    Title = "Book 1",
                    Description = "Description of Book 1",
                    Pages = 281,
                    AuthorId = new Guid("00000000-0000-0000-0000-000000000000")
                },
                new BookDto
                {
                    Id = new Guid("00000000-0000-0000-0000-000000000011"),
                    Title = "Book 2",
                    Description = "Description of Book 2",
                    Pages = 370,
                    AuthorId = new Guid("00000000-0000-0000-0000-000000000000")
                },
                new BookDto
                {
                    Id = new Guid("00000000-0000-0000-0000-000000000030"),
                    Title = "Book 3",
                    Description = "Description of Book 3",
                    Pages = 229,
                    AuthorId = new Guid("00000000-0000-0000-0000-000000000000")
                },
                new BookDto
                {
                    Id = new Guid("00000000-0000-0000-0000-000000000040"),
                    Title = "Book 4",
                    Description = "Description of Book 4",
                    Pages = 440,
                    AuthorId = new Guid("00000000-0000-0000-0000-000000000001")
                }
            };
        }
    }
}
