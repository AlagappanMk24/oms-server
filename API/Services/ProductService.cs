using API.Data.Repositories;
using API.Data.Repositories.Interface;
using API.DTOs;
using API.Entities;
using API.Services.Interface;
using AutoMapper;

namespace API.Services
{
    public class ProductService(IProductRepository productRepository, IMapper mapper, ILogger<ProductService>logger) : IProductService
    {
        private readonly IProductRepository _productRepository = productRepository;
        private readonly IMapper _mapper = mapper;
        private readonly ILogger<ProductService> _logger= logger;
        public async Task<IEnumerable<Product>> GetProductsAsync(int pageNumber, int pageSize)
        {
            try
            {
                return await _productRepository.GetAllProductsAsync(pageNumber, pageSize);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching products.");
                throw;
            }
        }

        public async Task<Product> GetProductByIdAsync(int id)
        {
            try
            {
                return await _productRepository.GetProductByIdAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching product with ID {ProductId}.", id);
                throw;
            }
        }

        public async Task<Product> CreateProductAsync(ProductDto productDto)
        {
            try
            {
                var product = _mapper.Map<Product>(productDto);
                product.CreatedDate = DateTime.UtcNow;
                product.UpdatedDate = DateTime.UtcNow;
                var createdProduct = await _productRepository.AddProductAsync(product);
                return createdProduct;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating a product.");
                throw;
            }
        }

        public async Task UpdateProductAsync(int id, Product product)
        {
            try
            {
                if (id != product.Id)
                {
                    throw new ArgumentException("Product ID mismatch.");
                }
                await _productRepository.UpdateProductAsync(product);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating product with ID {ProductId}.", id);
                throw;
            }
        }
        public async Task DeleteProductAsync(int id)
        {
            try
            {
                await _productRepository.DeleteProductAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting product with ID {ProductId}.", id);
                throw;
            }
        }
        public async Task<int> GetTotalProductCountAsync() 
        {
            try
            {
                return await _productRepository.GetTotalProductCountAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching total product count.");
                throw;
            }
        }
    }
}
