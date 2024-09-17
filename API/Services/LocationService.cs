using API.Data.Repositories.Interface;
using API.Entities;
using API.Services.Interface;

namespace API.Services
{
    public class LocationService(ILocationRepository locationRepository, ILogger<LocationService> logger) : ILocationService
    {
        private readonly ILocationRepository _locationRepository = locationRepository;
        private readonly ILogger<LocationService> _logger = logger;

        public async Task<IEnumerable<Location>> GetLocationsAsync()
        {
            try
            {
                return await _locationRepository.GetLocationsAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving locations.");
                throw;
            }
        }

        public async Task<Location> GetLocationByIdAsync(int locationId)
        {
            try
            {
                return await _locationRepository.GetLocationByIdAsync(locationId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving locations.");
                throw;
            }
        }
    }
}
