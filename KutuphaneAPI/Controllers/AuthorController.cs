using KutuphaneCore.DTOs;
using KutuphaneService.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KutuphaneAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class AuthorController : ControllerBase
    {
        private readonly IAuthorService _authorService;

        public AuthorController(IAuthorService authorService)
        {
            _authorService = authorService;
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult GetAll()
        {
            var result = _authorService.ListAll();
            if (!result.IsSuccess)
                return NotFound(result.Message);

            return Ok(result);
        }

        [AllowAnonymous]
        [HttpGet("[action]")]
        public IActionResult GetById(int id)
        {
            var result = _authorService.GetById(id);
            if (!result.IsSuccess)
                return NotFound(result.Message);

            return Ok(result);
        }

        [AllowAnonymous]
        [HttpGet("[action]")]
        public IActionResult GetByName(string name)
        {
            var result = _authorService.GetByName(name);
            if (!result.IsSuccess)
                return NotFound(result.Message);

            return Ok(result);
        }

        [HttpPost]
        public IActionResult Create(AuthorCreateDto author)
        {
            if (author == null)
                return BadRequest("Yazar bilgileri boş olamaz.");

            var result = _authorService.Create(author).Result;
            if (!result.IsSuccess)
                return BadRequest(result.Message);

            return Ok(result);
        }

        [HttpPut]
        public IActionResult Update(AuthorUpdateDto author)
        {
            if (author == null)
                return BadRequest("Yazar bilgileri boş olamaz.");

            var result = _authorService.Update(author).Result;
            if (!result.IsSuccess)
                return BadRequest(result.Message);

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
    }
}
