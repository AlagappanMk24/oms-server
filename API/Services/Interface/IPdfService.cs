using API.DTOs;
using API.Entities;

namespace API.Services.Interface
{
    public interface IPdfService
    {
        byte[] GenerateInvoicePdf(Invoice invoice);
    }
}
