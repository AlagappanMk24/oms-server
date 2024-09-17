using API.Entities;

namespace API.Services.Interface
{
    public interface ILocationService
    {
        Task<IEnumerable<Location>> GetLocationsAsync();
        Task<Location> GetLocationByIdAsync(int id);
    }
}
