using API.DTOs;
using API.Entities;
using API.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController(IOrderService orderService, ILogger<OrderController> logger) : ControllerBase
    {
        private readonly IOrderService _orderService = orderService;
        private readonly ILogger<OrderController> _logger = logger;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Order>>> GetAllOrders()
        {
            try
            {
                _logger.LogInformation("Getting all orders");
                var orders = await _orderService.GetAllOrdersAsync();
                return Ok(orders);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting all orders");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetOrderById(int id)
        {
            try
            {
                _logger.LogInformation("Getting order with ID {Id}", id);
                var order = await _orderService.GetOrderByIdAsync(id);

                if (order == null)
                {
                    _logger.LogWarning("Order with ID {Id} not found", id);
                    return NotFound();
                }

                return Ok(order);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting order with ID {Id}", id);
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<ActionResult<Order>> CreateOrder(OrderDto order)
        {
            try
            {
                _logger.LogInformation("Creating a new order");
                var createdOrder = await _orderService.CreateOrderAsync(order);
                return CreatedAtAction(nameof(GetOrderById), new { id = createdOrder.Id }, createdOrder);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating a new order");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrder(int id, Order order)
        {
            try
            {
                if (id != order.Id)
                {
                    _logger.LogWarning("Order ID mismatch: {Id} does not match {OrderId}", id, order.Id);
                    return BadRequest("Order ID mismatch");
                }

                var existingOrder = await _orderService.GetOrderByIdAsync(id);

                if (existingOrder == null)
                {
                    _logger.LogWarning("Order with ID {Id} not found", id);
                    return NotFound();
                }
                _logger.LogInformation("Updating order with ID {Id}", id);
                await _orderService.UpdateOrderAsync(order);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating order with ID {Id}", id);
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            try
            {
                var existingOrder = await _orderService.GetOrderByIdAsync(id);

                if (existingOrder == null)
                {
                    _logger.LogWarning("Order with ID {Id} not found", id);
                    return NotFound();
                }
                _logger.LogInformation("Deleting order with ID {Id}", id);
                await _orderService.DeleteOrderAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting order with ID {Id}", id);
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
