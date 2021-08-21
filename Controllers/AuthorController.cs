using AutoMapper;
using Library.API.Entities;
using Library.API.Helpers;
using Library.API.Models;
using Library.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library.API.Controllers
{
    [Route("api/authors")]
    [ApiController]
    [Authorize]
    public class AuthorController : ControllerBase
    {
        
        public IMapper Mapper { get; }

        public IRepositoryWrapper RepositoryWrapper { get; }

        public AuthorController(IRepositoryWrapper repositoryWrapper, IMapper mapper)
        {
            RepositoryWrapper = repositoryWrapper;
            Mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AuthorDto>>> GetAuthorsAsync([FromQuery] AuthorResourceParameters parameters)
        {
            var authors = (await RepositoryWrapper.Author.GetAllAsync())
                .Skip(parameters.PageSize * (parameters.PageNumber -1))
                .Take(parameters.PageSize)
                .OrderBy(author => author.Name);

            var authorDtoList = Mapper.Map<IEnumerable<AuthorDto>>(authors);

            return authorDtoList.ToList();
        }

        [HttpGet("{authorId}", Name = nameof(GetAuthorAsync))]
        public async Task<ActionResult<AuthorDto>> GetAuthorAsync(Guid authorId)
        {
            var author = await RepositoryWrapper.Author.GetByIdAsync(authorId);

            if(author == null)
            {
                return NotFound();
            }
            else
            {
                var authorDto = Mapper.Map<AuthorDto>(author);

                return authorDto;
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateAuthorAsync(AuthorForCreationDto authorForCreationDto)
        {
            var author = Mapper.Map<Author>(authorForCreationDto);

            RepositoryWrapper.Author.Create(author);

            var result = await RepositoryWrapper.Author.SaveAsync();

            if(!result)
            {
                throw new Exception("创建资源autor失败");
            }

            var authorDto = Mapper.Map<AuthorDto>(author);

            return CreatedAtRoute(nameof(GetAuthorAsync), new { authorId = authorDto.Id }, authorDto);

        }

        [HttpDelete("{authorId}")]
        public async Task<IActionResult> DeleteAuthorAsync(Guid authorId)
        {
            var author = await RepositoryWrapper.Author.GetByIdAsync(authorId);

            if (author == null)
            {
                return NotFound();
            }

            RepositoryWrapper.Author.Delete(author);

            var result = await RepositoryWrapper.Author.SaveAsync();

            if (!result)
            {
                throw new Exception("删除资源author失败");
            }

            return NoContent();
        }
    }
}
