using API.DTOs;
using API.Entities;

namespace API.Services.Interface
{
    public interface IInvoiceService
    {
        Task<Invoice> GetInvoiceAsync(int id);
        Task<IEnumerable<InvoiceDto>> GetAllInvoicesAsync();
        Task<Invoice> AddInvoiceAsync(InvoiceDto invoiceDto);
        Task UpdateInvoiceAsync(Invoice invoice);
        Task DeleteInvoiceAsync(int id);
    }
}
