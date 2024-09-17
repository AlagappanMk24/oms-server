using API.Data.Context;
using API.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocationController(AppDbContext context, ILogger<LocationController> logger, ILocationService locationService) : ControllerBase
    {
        private readonly AppDbContext _context = context;
        private readonly ILogger<LocationController> _logger = logger;
        private readonly ILocationService _locationService = locationService;

        [HttpGet]
        public async Task<IActionResult> GetLocations()
        {
            try
            {
                var locations = await _locationService.GetLocationsAsync();
                return Ok(locations);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving locations.");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving locations.");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetLocationById(int id)
        {
            try
            {
                var locationById = await _locationService.GetLocationByIdAsync(id);
                return Ok(locationById);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving locations.");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving locations.");
            }
        }
    }
}
