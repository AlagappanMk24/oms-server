using API.Entities;

namespace API.Data.Repositories.Interface
{
    public interface ILocationRepository
    {
        Task<IEnumerable<Location>> GetLocationsAsync();
        Task<Location> GetLocationByIdAsync(int id);
    }
}
