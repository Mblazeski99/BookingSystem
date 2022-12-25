using BookingSystem.Models;
using BookingSystem.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookingSystem.Controllers
{
    [ApiController]
    [Route("{controller}")]
    public class AuthController : Controller
    {
        private readonly JWTAuthenticationManager _jWTAuthenticationManager;

        public AuthController(JWTAuthenticationManager jWTAuthenticationManager)
        {
            _jWTAuthenticationManager = jWTAuthenticationManager;
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Index()
        {
            return Ok("Go to search to check out avaliable hotels and flights");
        }

        [AllowAnonymous]
        [HttpPost("Authorize")]
        public IActionResult AuthUser([FromBody] User user)
        {
            var token = _jWTAuthenticationManager.Authenticate(user.Username, user.Password);
            if (token == null) 
            {
                return Unauthorized();
            }

            return Ok(token);
        }
    }
}
