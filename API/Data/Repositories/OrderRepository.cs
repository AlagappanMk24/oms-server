using API.Data.Context;
using API.Data.Repositories.Interface;
using API.Entities;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace API.Data.Repositories
{
    public class OrderRepository(AppDbContext context) : IOrderRepository
    {
        private readonly AppDbContext _context = context;
        public async Task<IEnumerable<Order>> GetAllOrdersAsync()
        {
            return await _context.Orders
              .Include(o => o.Customer)
              .Include(o => o.OrderItems)
                  .ThenInclude(oi => oi.Product)
              .ToListAsync();
        }
        public async Task<Order> GetOrderByIdAsync(int id)
        {
            return await _context.Orders
               .Include(o => o.Customer)
               .Include(o => o.OrderItems)
                   .ThenInclude(oi => oi.Product)
               .FirstOrDefaultAsync(o => o.Id == id);
        }
        public async Task<Order> CreateOrderAsync(Order order)
        {
            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();
            return order;
        }
        public async Task UpdateOrderAsync(Order order)
        {
            _context.Entry(order).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
        public async Task DeleteOrderAsync(int id)
        {
            var order = await _context.Orders.FindAsync(id);

            if (order != null)
            {
                _context.Orders.Remove(order);
                await _context.SaveChangesAsync();
            }
        }
        public async Task<Customer?> GetCustomerByIdAsync(int customerId)
        {
            return await _context.Customers.FindAsync(customerId);
        }
        public async Task<Product?> GetProductByIdAsync(int productId)
        {
            return await _context.Products.FindAsync(productId);
        }
    }
}
