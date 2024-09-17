using API.Data.Repositories.Interface;
using API.DTOs;
using API.Entities;
using API.Services.Interface;
using AutoMapper;

namespace API.Services
{
    public class CustomerService(ICustomerRepository customerRepository, IMapper mapper, ILogger<CustomerService> logger) : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository = customerRepository;
        private readonly IMapper _mapper = mapper;
        private readonly ILogger<CustomerService> _logger = logger;

        public async Task<IEnumerable<CustomerDto>> GetCustomersAsync()
        {
            try
            {
                var customers = await _customerRepository.GetCustomersAsync();
                return customers.Select(c => _mapper.Map<CustomerDto>(c)).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting customers.");
                throw;
            }
        }

        public async Task<CustomerDto> GetCustomerByIdAsync(int id)
        {
            try
            {
                var customer = await _customerRepository.GetCustomerByIdAsync(id);
                return _mapper.Map<CustomerDto>(customer);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting customer with ID {CustomerId}.", id);
                throw;
            }
        }

        public async Task<Customer> AddCustomerAsync(CustomerDto customerDto)
        {
            try
            {
                var customer = _mapper.Map<Customer>(customerDto);
                return await _customerRepository.AddCustomerAsync(customer);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding a new customer.");
                throw;
            }
        }

        public async Task<Customer> UpdateCustomerAsync(int id, CustomerDto customerDto)
        {
            if (id != customerDto.Id)
            {
                _logger.LogWarning("ID mismatch: ID in URL is {Id} but ID in body is {CustomerDtoId}.", id, customerDto.Id);
                throw new ArgumentException("ID mismatch");
            }

            try
            {
                var customer = await _customerRepository.GetCustomerByIdAsync(id);
                if (customer == null)
                {
                    _logger.LogError("Customer with ID {CustomerId} not found for update.", id);
                    throw new Exception("Customer not found");
                }

                _mapper.Map(customerDto, customer);
                var updatedCustomer = await _customerRepository.UpdateCustomerAsync(customer);
                return updatedCustomer;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating customer with ID {CustomerId}.", id);
                throw; 
            }
        }

        public async Task DeleteCustomerAsync(int id)
        {
            try
            {
                var customer = await _customerRepository.GetCustomerByIdAsync(id);
                if (customer == null)
                {
                    _logger.LogWarning("Attempted to delete a non-existent customer with ID {CustomerId}.", id);
                    throw new ArgumentException("Customer not found");
                }

                await _customerRepository.DeleteCustomerAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting customer with ID {CustomerId}.", id);
                throw;
            }
        }

        public async Task<bool> CustomerExistsAsync(int id)
        {
            try
            {
                return await _customerRepository.CustomerExistsAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while checking if customer with ID {CustomerId} exists.", id);
                throw;
            }
        }
    }
}
