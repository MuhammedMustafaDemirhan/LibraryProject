using KutuphaneCore.DTOs;
using KutuphaneCore.Entities;
using KutuphaneService.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KutuphaneAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorController : ControllerBase
    {
        private readonly IAuthorService _authorService;
        public AuthorController(IAuthorService authorService)
        {
            _authorService = authorService;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var authors = _authorService.ListAll();

            if (!authors.IsSuccess)
                return NotFound(authors.Message);

            return Ok(authors);
        }

        [HttpGet("GetById")]
        public IActionResult GetById(int id)
        {
            var author = _authorService.GetById(id);

            if (!author.IsSuccess)
                return NotFound(author.Message);

            return Ok(author);
        }

        [HttpGet("GetByName")]
        public IActionResult GetByName(string name)
        {
            var result = _authorService.GetByName(name);

            if (!result.IsSuccess)
                return NotFound(result.Message);

            return Ok(result);
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var result = _authorService.Delete(id);

            if (!result.IsSuccess)
                return BadRequest(result.Message);

            return Ok(result);
        }

        [HttpPost]
        public IActionResult Create([FromBody]AuthorCreateDto author)
        {
            if (author == null)
                return BadRequest("Yazar bilgileri boş olamaz.");

            var result = _authorService.Create(author).Result;

            if (!result.IsSuccess)
                return BadRequest(result.Message);

            return Ok(result);
        }

        [HttpPut]
        public IActionResult Update([FromBody]AuthorUpdateDto author)
        {
            if (author == null)
                return BadRequest("Yazar bilgileri boş olamaz.");

            var result = _authorService.Update(author).Result;

            if (!result.IsSuccess)
                return BadRequest(result.Message);

            return Ok(result);
        }
    }
}
