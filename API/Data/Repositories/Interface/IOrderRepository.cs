using API.DTOs;
using API.Entities;

namespace API.Data.Repositories.Interface
{
    public interface IOrderRepository
    {
        Task<IEnumerable<Order>> GetAllOrdersAsync();
        Task<Order> GetOrderByIdAsync(int id);
        Task<Order> CreateOrderAsync(Order order);
        Task UpdateOrderAsync(Order order);
        Task DeleteOrderAsync(int id);
        Task<Customer?> GetCustomerByIdAsync(int customerId);
        Task<Product?> GetProductByIdAsync(int productId);
    }
}
