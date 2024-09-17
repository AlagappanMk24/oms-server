using API.Entities;

namespace API.Data.Repositories.Interface
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAllProductsAsync(int pageNumber, int pageSize);
        Task<Product> GetProductByIdAsync(int id);
        Task<Product> AddProductAsync(Product product);
        Task UpdateProductAsync(Product product);
        Task DeleteProductAsync(int id);
        Task<int> GetTotalProductCountAsync();
    }
}
