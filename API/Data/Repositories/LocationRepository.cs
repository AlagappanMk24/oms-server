using API.Data.Context;
using API.Data.Repositories.Interface;
using API.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Data.Repositories
{
    public class LocationRepository(AppDbContext dbContext) : ILocationRepository
    {
        private readonly AppDbContext _dbContext = dbContext;
        public async Task<IEnumerable<Location>> GetLocationsAsync()
        {
            return await _dbContext.Locations
                      .Include(c => c.Currency)
                      .Include(tz => tz.Timezone)
                      .ToListAsync();
        }
        public async Task<Location> GetLocationByIdAsync(int id)
        {
            return await _dbContext.Locations
                       .Include(c => c.Currency)
                       .Include(tz => tz.Timezone)
                       .FirstOrDefaultAsync(l => l.Id == id);
        }
    }
}
