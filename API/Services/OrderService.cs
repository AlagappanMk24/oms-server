using API.Data.Repositories.Interface;
using API.DTOs;
using API.Entities;
using API.Services.Interface;
using AutoMapper;

namespace API.Services
{
    public class OrderService(IOrderRepository orderRepository, IMapper mapper, ILogger<OrderService>logger) : IOrderService
    {
        private readonly IOrderRepository _orderRepository = orderRepository;
        private readonly IMapper _mapper = mapper;
        private readonly ILogger<OrderService> _logger = logger;
        public async Task<IEnumerable<Order>> GetAllOrdersAsync()
        {
            try
            {
                return await _orderRepository.GetAllOrdersAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving all orders.");
                throw;
            }
        }
        public async Task<Order> GetOrderByIdAsync(int id)
        {
            try
            {
                return await _orderRepository.GetOrderByIdAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while retrieving order with ID {id}.");
                throw;
            }
        }
        public async Task<Order> CreateOrderAsync(OrderDto orderDto)
        {
            try
            {
                // Fetch the customer from the database
                var customer = await _orderRepository.GetCustomerByIdAsync(orderDto.CustomerId);
                if (customer == null)
                {
                    throw new Exception("Customer not found.");
                }

                decimal totalAmount = 0;

                // Validate each OrderItem
                foreach (var item in orderDto.OrderItems)
                {
                    var product = await _orderRepository.GetProductByIdAsync(item.ProductId);
                    if (product == null)
                    {
                        throw new Exception($"Product with ID {item.ProductId} not found.");
                    }
                    item.TotalPrice = item.Quantity * item.UnitPrice;
                    totalAmount += item.TotalPrice;
                }

                totalAmount += orderDto.ShippingCost;
                totalAmount += orderDto.TaxAmount;
                totalAmount -= orderDto.Discount;
                orderDto.TotalAmount = totalAmount;

                var order = _mapper.Map<Order>(orderDto);

                // Map order items
                foreach (var item in orderDto.OrderItems)
                {
                    var orderItem = _mapper.Map<OrderItem>(item);
                    orderItem.Order = order; // Set navigation property
                    order.OrderItems.Add(orderItem);
                }

                return await _orderRepository.CreateOrderAsync(order);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating an order.");
                throw;
            }
        }

        public async Task UpdateOrderAsync(Order order)
        {
            try
            {
                await _orderRepository.UpdateOrderAsync(order);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while updating order with ID {order.Id}.");
                throw;
            }
        }
        public async Task DeleteOrderAsync(int id)
        {
            try
            {
                await _orderRepository.DeleteOrderAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while deleting order with ID {id}.");
                throw;
            }
        }
    }
}
