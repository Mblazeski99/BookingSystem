using BookingSystem.Domain.Models;
using BookingSystem.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookingSystem.Controllers
{
    [ApiController]
    [Route("{controller}")]
    public class BookController : Controller
    {
        private readonly IManagerService _managerService;

        public BookController(IManagerService managerService)
        {
            _managerService = managerService;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Index(string optionCode)
        {
            var bookRequest = new BookRequest()
            {
                OptionCode = optionCode
            };

            var result = await _managerService.Book(bookRequest);

            string message = $@"Succes! Your booking is currently pending, you can check the status using this code: {result.BookingCode}
Booking Time: {result.BookingTime}";

            return Ok(message);
        }
    }
}