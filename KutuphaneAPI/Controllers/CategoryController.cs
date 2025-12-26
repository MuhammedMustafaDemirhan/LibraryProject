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
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [AllowAnonymous]
        [EnableRateLimiting("RateLimiter")]
        [HttpGet]
        public IActionResult GetAll()
        {
            var result = _categoryService.ListAll();
            if (!result.IsSuccess)
                return NotFound(result.Message);

            return Ok(result);
        }

        [AllowAnonymous]
        [EnableRateLimiting("RateLimiter")]
        [HttpGet("[action]")]
        public IActionResult GetById(int id)
        {
            var result = _categoryService.GetById(id);
            if (!result.IsSuccess)
                return NotFound(result.Message);

            return Ok(result);
        }

        [AllowAnonymous]
        [EnableRateLimiting("RateLimiter")]
        [HttpGet("[action]")]
        public IActionResult GetByName(string name)
        {
            var result = _categoryService.GetByName(name);
            if (!result.IsSuccess)
                return NotFound(result.Message);

            return Ok(result);
        }

        [HttpPost]
        public IActionResult Create(CategoryCreateDto category)
        {
            if (category == null)
                return BadRequest("Kategori bilgileri boş olamaz.");

            var result = _categoryService.Create(category).Result;
            if (!result.IsSuccess)
                return BadRequest(result.Message);

            return Ok(result);
        }

        [HttpPut]
        public IActionResult Update(CategoryUpdateDto categoryUpdateDto)
        {
            if (categoryUpdateDto == null)
                return BadRequest("Kategori bilgileri boş olamaz.");

            var result = _categoryService.Update(categoryUpdateDto).Result;
            if (!result.IsSuccess)
                return BadRequest(result.Message);

            return Ok(result);
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var result = _categoryService.Delete(id);
            if (!result.IsSuccess)
                return BadRequest(result.Message);

            return Ok(result);
        }
    }
}
