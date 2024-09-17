using API.Entities;

namespace API.Data.Repositories.Interface
{
    public interface IInvoiceRepository
    {
        Task<Invoice> GetInvoiceAsync(int id);
        Task<IEnumerable<Invoice>> GetAllInvoicesAsync();
        Task<Invoice> AddInvoiceAsync(Invoice invoice);
        Task UpdateInvoiceAsync(Invoice invoice);
        Task DeleteInvoiceAsync(int id);
    }
}
