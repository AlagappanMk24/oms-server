using API.DTOs;
using API.Entities;

namespace API.Services.Interface
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> GetProductsAsync(int pageNumber, int pageSize);
        Task<Product> GetProductByIdAsync(int id);
        Task<Product> CreateProductAsync(ProductDto productDto);
        Task UpdateProductAsync(int id, Product product);
        Task DeleteProductAsync(int id);
        Task<int> GetTotalProductCountAsync();
    }
}
