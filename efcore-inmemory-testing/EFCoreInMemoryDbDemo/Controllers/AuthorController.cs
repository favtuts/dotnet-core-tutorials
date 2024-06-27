using EFCoreInMemoryDbDemo.DTOs;
using EFCoreInMemoryDbDemo.Models;
using EFCoreInMemoryDbDemo.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace EFCoreInMemoryDbDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorController : ControllerBase
    {
        readonly IAuthorRepository _authorRepository;
        public AuthorController(IAuthorRepository authorRepository)
        {
            _authorRepository = authorRepository;
        }
        [HttpGet]
        public ActionResult<List<Author>> Get()
        {
            List<AuthorDTO>? resultList = null;
            List<Author> authors = _authorRepository.GetAuthors();
            if (authors!=null)
            {
                resultList = new List<AuthorDTO>();
                foreach(Author at in authors)
                {
                    AuthorDTO atDTO = new AuthorDTO();
                    atDTO.Id = at.Id;
                    atDTO.FirstName = at.FirstName;
                    atDTO.LastName = at.LastName;
                    if (at.Books!=null)
                    {
                        atDTO.Books = new List<BookDTO>();
                        foreach(Book bk in at.Books)
                        {
                            BookDTO bkDTO = new BookDTO();
                            bkDTO.Title = bk.Title;                            
                            bkDTO.AuthorId = bk.Author != null? bk.Author.Id: -1;
                            atDTO.Books.Add(bkDTO);
                        }
                    }
                    resultList.Add(atDTO);
                }
            }
            return Ok(resultList);
        }
    }
}
