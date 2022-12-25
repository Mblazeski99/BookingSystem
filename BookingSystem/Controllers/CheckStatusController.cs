using BookingSystem.Domain.Models;
using BookingSystem.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookingSystem.Controllers
{
    [Authorize]
    [ApiController]
    [Route("{controller}")]
    public class CheckStatusController : Controller
    {
        private readonly IManagerService _managerService;

        public CheckStatusController(IManagerService managerService)
        {
            _managerService = managerService;
        }

        public async Task<IActionResult> Index(string bookingCode)
        {
            var bookingRequest = new CheckStatusRequest() { BookingCode = bookingCode };

            var result = await _managerService.CheckStatus(bookingRequest);
            return Ok(result.Status.ToString());
        }
    }
}
