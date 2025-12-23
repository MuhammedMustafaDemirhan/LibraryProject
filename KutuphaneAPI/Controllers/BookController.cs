using KutuphaneDataAccess.DTOs;
using KutuphaneService.Interfaces;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KutuphaneAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly IBookService _bookService;
        public BookController(IBookService bookService)
        {
            _bookService = bookService;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var books = _bookService.ListAll();

            if(!books.IsSuccess)
                return NotFound(books.Message);

            return Ok(books);
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var result = _bookService.Delete(id);

            if (!result.IsSuccess)
                return BadRequest(result.Message);

            return Ok(result);
        }

        [HttpPost]
        public IActionResult Create([FromBody]BookCreateDto book)
        {
            if (book == null)
                return BadRequest("Kitap bilgileri boş olamaz.");

            var result = _bookService.Create(book).Result;

            if (!result.IsSuccess)
                return BadRequest(result.Message);

            return Ok(result);
        }

        [HttpGet("GetById")]
        public IActionResult GetById(int id)
        {
            var result = _bookService.GetById(id);

            if (!result.IsSuccess)
                return NotFound(result.Message);

            return Ok(result);
        }

        [HttpGet("GetByName")]
        public IActionResult GetByName(string name)
        {
            var result = _bookService.GetByName(name);

            if (!result.IsSuccess)
                return NotFound(result.Message);

            return Ok(result);
        }

        [HttpGet("GetBooksByCategoryId")]
        public IActionResult GetBooksByCategoryId(int categoryId)
        {
            var result = _bookService.GetBooksByCategoryId(categoryId);

            if (!result.IsSuccess)
                return NotFound(result.Message);

            return Ok(result);
        }

        [HttpGet("GetBooksByAuthorId")]
        public IActionResult GetBooksByAuthorId(int authorId)
        {
            var result = _bookService.GetBooksByAuthorId(authorId);

            if (!result.IsSuccess)
                return NotFound(result.Message);

            return Ok(result);
        }

        [HttpPut]
        public IActionResult Update([FromBody]BookUpdateDto bookUpdateDto)
        {
            if (bookUpdateDto == null)
                return BadRequest("Kitap bilgileri boş olamaz.");

            var result = _bookService.Update(bookUpdateDto).Result;

            if (!result.IsSuccess)
                return BadRequest(result.Message);

            return Ok(result);
        }
    }
}
