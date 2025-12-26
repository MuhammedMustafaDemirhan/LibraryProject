using KutuphaneDataAccess.DTOs;
using KutuphaneService.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace KutuphaneAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [EnableRateLimiting("RateLimiter")]
        [HttpPost("Register")]
        public IActionResult CreateUser([FromBody] UserCreateDto user)
        {
            if (user == null)
                return BadRequest("Kullanıcı bilgileri boş olamaz.");

            var result = _userService.Create(user);

            if (!result.IsSuccess)
                return BadRequest(result.Message);

            return Ok(result);
        }

        [EnableRateLimiting("RateLimiter")]
        [HttpPost("Login")]
        public IActionResult LoginUser([FromBody] UserLoginbDto user)
        {
            if (user == null)
                return BadRequest("Kullanıcı bilgileri boş olamaz.");

            var result = _userService.LoginUser(user);

            if (!result.IsSuccess)
                return BadRequest(result.Message);

            return Ok(result);
        }
    }
}
