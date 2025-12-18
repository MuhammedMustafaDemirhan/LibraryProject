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

            if (authors == null)
                return BadRequest("Yazar bulunamadı.");

            return Ok(authors);
        }

        [HttpGet("GetByName")]
        public IActionResult GetByName(string name)
        {
            if (name == null)
                return BadRequest("Ad alanı boş bırakılamaz.");

            var result = _authorService.GetByName(name);

            if (result == null)
                return BadRequest("Yazar bulunamadı.");

            return Ok(result);
        }

        [HttpGet("GetById")]
        public IActionResult GetById(int id)
        {
            if (id == null)
                return BadRequest("Geçerli bir Id değeri giriniz.");

            var author = _authorService.GetById(id);

            if (author == null)
                return BadRequest("Yazar bulunamadı.");

            return Ok(author);
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var result = _authorService.Delete(id);

            if (result == null)
                return BadRequest("Silme işlemi başarısız oldu.");

            return Ok(result);
        }

        [HttpPost]
        public IActionResult Create([FromBody]Author author)
        {
            if (author == null)
                return BadRequest("Yazar bilgileri boş olamaz.");

            var result = _authorService.Create(author);

            if (result == null)
                return BadRequest("Yazar oluşturulamadı.");

            return Ok(result);
        }
    }
}
