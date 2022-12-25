using BookingSystem.Domain.Models;
using BookingSystem.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookingSystem.Controllers
{
    [Authorize]
    [ApiController]
    [Route("{controller}")]
    public class SearchController : Controller
    {
        private readonly IManagerService _managerService;

        public SearchController(IManagerService managerService)
        {
            _managerService = managerService;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Index(string destination, DateTime from, DateTime to, string? departureAirport = null)
        {
            var searchRequest = new SearchRequest()
            {
                Destination = destination,
                FromDate = from,
                ToDate = to,
                DepartureAirport = departureAirport ?? string.Empty
            };

            var result = await _managerService.Search(searchRequest);
            return Ok(result.Options);
        }
    }
}
