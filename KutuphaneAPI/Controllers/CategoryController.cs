using KutuphaneDataAccess.DTOs;
using KutuphaneService.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KutuphaneAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var categories = _categoryService.ListAll();

            if (!categories.IsSuccess)
                return NotFound(categories.Message);

            return Ok(categories);
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var result = _categoryService.Delete(id);

            if (!result.IsSuccess)
                return BadRequest(result.Message);

            return Ok(result);
        }

        [HttpPost]
        public IActionResult Create([FromBody]CategoryCreateDto category)
        {
            if (category == null)
                return BadRequest("Kategori bilgileri boş olamaz.");

            var result = _categoryService.Create(category).Result;

            if (!result.IsSuccess)
                return BadRequest(result.Message);

            return Ok(result);
        }

        [HttpGet("GetById")]
        public IActionResult GetById(int id)
        {
            var result = _categoryService.GetById(id);

            if (!result.IsSuccess)
                return NotFound(result.Message);

            return Ok(result);
        }

        [HttpGet("GetByName")]
        public IActionResult GetByName(string name)
        {
            var result = _categoryService.GetByName(name);

            if (!result.IsSuccess)
                return NotFound(result.Message);

            return Ok(result);
        }

        [HttpPut]
        public IActionResult Update([FromBody]CategoryUpdateDto categoryUpdateDto)
        {
            if (categoryUpdateDto == null)
                return BadRequest("Kategori bilgileri boş olamaz.");

            var result = _categoryService.Update(categoryUpdateDto).Result;

            if (!result.IsSuccess)
                return BadRequest(result.Message);

            return Ok(result);
        }
    }
}
