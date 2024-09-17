using API.Data.Context;
using API.Data.Repositories.Interface;
using API.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Data.Repositories
{
    public class ProductRepository(AppDbContext dbContext) : IProductRepository
    {
        private readonly AppDbContext _dbContext = dbContext;
        public async Task<IEnumerable<Product>> GetAllProductsAsync(int pageNumber, int pageSize)
        {
            return await _dbContext.Products
                .AsNoTracking()
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }
        public async Task<Product> GetProductByIdAsync(int id)
        {
            return await _dbContext.Products.FindAsync(id);
        }
        public async Task<Product> AddProductAsync(Product product)
        {
            await _dbContext.Products.AddAsync(product);
            await _dbContext.SaveChangesAsync();
            return product;
        }
        public async Task UpdateProductAsync(Product product)
        {
            _dbContext.Entry(product).State = EntityState.Modified;
        }
        public async Task DeleteProductAsync(int id)
        {
            var product = await _dbContext.Products.FindAsync(id);
            if (product != null)
            {
                _dbContext.Products.Remove(product);
            }
        }
        public async Task<int> GetTotalProductCountAsync()
        {
            return await _dbContext.Products.CountAsync();
        }
    }
}
