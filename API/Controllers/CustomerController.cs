using API.DTOs;
using API.Entities;
using API.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController(ICustomerService customerService, ILogger<CustomerController>logger) : ControllerBase
    {
        private readonly ICustomerService _customerService = customerService;
        private readonly ILogger<CustomerController> _logger = logger;

        // GET: api/Customer
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CustomerDto>>> GetCustomers()
        {
            try
            {
                _logger.LogInformation("Fetching all customers.");
                var customers = await _customerService.GetCustomersAsync();
                return Ok(customers);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching customers.");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // GET: api/Customer/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CustomerDto>> GetCustomer(int id)
        {
            try
            {
                _logger.LogInformation("Fetching customer with ID {CustomerId}.", id);
                var customer = await _customerService.GetCustomerByIdAsync(id);
                if (customer == null)
                {
                    _logger.LogWarning("Customer with ID {CustomerId} not found.", id); 
                    return NotFound();
                }
                return Ok(customer);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching customer with ID {CustomerId}.", id);
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // POST: api/Customer
        [HttpPost]
        public async Task<ActionResult<Customer>> CreateCustomer(CustomerDto customerDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("Model state is invalid for customer creation.");
                    return BadRequest(ModelState);
                }
                _logger.LogInformation("Creating new customer.");
                var createdCustomer = await _customerService.AddCustomerAsync(customerDto);
                _logger.LogInformation("Customer created with ID {CustomerId}.", createdCustomer.Id);
                return CreatedAtAction(nameof(GetCustomer), new { id = createdCustomer.Id }, createdCustomer);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating a new customer.");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // PUT: api/Customer/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCustomer(int id, CustomerDto customerDto)
        {
            if (id != customerDto.Id)
            {
                _logger.LogWarning("ID mismatch: provided ID {ProvidedId} does not match customer ID {CustomerId}.", id, customerDto.Id);
                return BadRequest("Customer ID mismatch");
            }
            try
            {
                _logger.LogInformation("Updating customer with ID {CustomerId}.", id);
                var updatedCustomer = await _customerService.UpdateCustomerAsync(id, customerDto);
                return Ok(updatedCustomer);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Invalid argument while updating customer with ID {CustomerId}.", id);
                return BadRequest();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating customer with ID {CustomerId}.", id);
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // DELETE: api/Customer/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomer(int id)
        {
            try
            {
                _logger.LogInformation("Attempting to delete customer with ID {CustomerId}.", id);
                var customerExists = await _customerService.CustomerExistsAsync(id);
                if (!customerExists)
                {
                    _logger.LogWarning("Customer with ID {CustomerId} not found for deletion.", id);
                    return NotFound();
                }
                await _customerService.DeleteCustomerAsync(id);
                _logger.LogInformation("Customer with ID {CustomerId} successfully deleted.", id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting customer with ID {CustomerId}.", id);
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}

