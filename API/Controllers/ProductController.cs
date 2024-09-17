using API.DTOs;
using API.Entities;
using API.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController(IProductService productService, ILogger<ProductController>logger) : ControllerBase
    {
        private readonly IProductService _productService = productService;
        private readonly ILogger<ProductController> _logger = logger;

        [HttpGet]
        public async Task<IActionResult> Products([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                _logger.LogInformation("Fetching products for page number {PageNumber} with page size {PageSize}.", pageNumber, pageSize);

                var totalRecords = await _productService.GetTotalProductCountAsync();

                var products = await _productService.GetProductsAsync(pageNumber, pageSize);

                var paginatedResult = new
                {
                    TotalRecords = totalRecords,
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    Products = products
                };

                _logger.LogInformation("Successfully fetched {TotalRecords} products.", totalRecords);

                return Ok(paginatedResult);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching products.");

                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProduct(int id)
        {
            try
            {
                _logger.LogInformation("Fetching product with ID {ProductId}.", id);

                var product = await _productService.GetProductByIdAsync(id);

                if (product == null)
                {
                    _logger.LogWarning("Product with ID {ProductId} not found.", id);
                    return NotFound("Product not found.");
                }

                _logger.LogInformation("Successfully fetched product with ID {ProductId}.", id);

                return Ok(product);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching product with ID {ProductId}.", id);

                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct(ProductDto productDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("Invalid model state for creating product.");
                    return BadRequest(ModelState);
                }

                if (productDto == null)
                {
                    _logger.LogWarning("Received null product DTO for creation.");
                    return BadRequest("Product is null.");
                }

                _logger.LogInformation("Creating a new product.");

                var createdProduct = await _productService.CreateProductAsync(productDto);

                _logger.LogInformation("Successfully created product with ID {ProductId}.", createdProduct.Id);

                return CreatedAtAction(nameof(GetProduct), new { id = createdProduct.Id }, createdProduct);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating a product.");

                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, Product product)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("Invalid model state for updating product with ID {ProductId}.", id);

                    return BadRequest(ModelState);
                }

                if (id != product.Id)
                {
                    _logger.LogWarning("Product ID mismatch: URL ID {UrlId} does not match body ID {BodyId}.", id, product.Id);

                    return BadRequest("Product ID mismatch.");
                }

                _logger.LogInformation("Updating product with ID {ProductId}.", id);

                await _productService.UpdateProductAsync(id, product);

                _logger.LogInformation("Successfully updated product with ID {ProductId}.", id);

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating product with ID {ProductId}.", id);

                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            try
            {
                _logger.LogInformation("Deleting product with ID {ProductId}.", id);

                var product = await _productService.GetProductByIdAsync(id);

                if (product == null)
                {
                    _logger.LogWarning("Product with ID {ProductId} not found.", id);

                    return NotFound("Product not found.");
                }

                await _productService.DeleteProductAsync(id);

                _logger.LogInformation("Successfully deleted product with ID {ProductId}.", id);

                return Ok(product);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting product with ID {ProductId}.", id);

                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}