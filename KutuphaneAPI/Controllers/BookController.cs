using KutuphaneDataAccess.DTOs;
using KutuphaneService.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace KutuphaneAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class BookController : ControllerBase
    {
        private readonly IBookService _bookService;

        public BookController(IBookService bookService)
        {
            _bookService = bookService;
        }

        [EnableRateLimiting("RateLimiter")]
        [AllowAnonymous]
        [HttpGet]
        public IActionResult GetAll()
        {
            var result = _bookService.ListAll();
            if (!result.IsSuccess)
                return NotFound(result.Message);

            return Ok(result);
        }

        [EnableRateLimiting("RateLimiter")]
        [AllowAnonymous]
        [HttpGet("[action]")]
        public IActionResult GetById(int id)
        {
            var result = _bookService.GetById(id);
            if (!result.IsSuccess)
                return NotFound(result.Message);

            return Ok(result);
        }

        [EnableRateLimiting("RateLimiter")]
        [AllowAnonymous]
        [HttpGet("[action]")]
        public IActionResult GetByName(string name)
        {
            var result = _bookService.GetByName(name);
            if (!result.IsSuccess)
                return NotFound(result.Message);

            return Ok(result);
        }

        [EnableRateLimiting("RateLimiter")]
        [AllowAnonymous]
        [HttpGet("[action]")]
        public IActionResult GetBooksByCategoryId(int categoryId)
        {
            var result = _bookService.GetBooksByCategoryId(categoryId);
            if (!result.IsSuccess)
                return NotFound(result.Message);

            return Ok(result);
        }

        [EnableRateLimiting("RateLimiter")]
        [AllowAnonymous]
        [HttpGet("[action]")]
        public IActionResult GetBooksByAuthorId(int authorId)
        {
            var result = _bookService.GetBooksByAuthorId(authorId);
            if (!result.IsSuccess)
                return NotFound(result.Message);

            return Ok(result);
        }

        [HttpPost]
        public IActionResult Create(BookCreateDto book)
        {
            if (book == null)
                return BadRequest("Kitap bilgileri boş olamaz.");

            var result = _bookService.Create(book).Result;
            if (!result.IsSuccess)
                return BadRequest(result.Message);

            return Ok(result);
        }

        [HttpPut]
        public IActionResult Update(BookUpdateDto bookUpdateDto)
        {
            if (bookUpdateDto == null)
                return BadRequest("Kitap bilgileri boş olamaz.");

            var result = _bookService.Update(bookUpdateDto).Result;
            if (!result.IsSuccess)
                return BadRequest(result.Message);

            return Ok(result);
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var result = _bookService.Delete(id);
            if (!result.IsSuccess)
                return BadRequest(result.Message);

            return Ok(result);
        }
    }
}
