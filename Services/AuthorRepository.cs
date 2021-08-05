using Library.API.Entities;
using Library.API.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library.API.Services
{
    public class AuthorRepository : RepositoryBase<Author, Guid>, IAuthorRepository
    {
        public AuthorRepository(DbContext dbContext) : base(dbContext)
        {

        }

        public void AddAuthor(AuthorDto author)
        {
            throw new NotImplementedException();
        }

        public void DeleteAuthor(AuthorDto author)
        {
            throw new NotImplementedException();
        }

        public AuthorDto GetAuthor(Guid authorId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<AuthorDto> GetAuthors()
        {
            throw new NotImplementedException();
        }

        public bool IsAuthorExists(Guid authorId)
        {
            throw new NotImplementedException();
        }
    }
}
