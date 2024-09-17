using API.DTOs;
using API.Entities;

namespace API.Services.Interface
{
    public interface ICustomerService
    {
        Task<IEnumerable<CustomerDto>> GetCustomersAsync();
        Task<CustomerDto> GetCustomerByIdAsync(int id);
        Task<Customer> AddCustomerAsync(CustomerDto customerDto);
        Task<Customer> UpdateCustomerAsync(int id, CustomerDto customerDto);
        Task DeleteCustomerAsync(int id);
        Task<bool> CustomerExistsAsync(int id);
    }
}
