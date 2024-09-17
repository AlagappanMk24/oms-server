using API.Data.Repositories;
using API.Data.Repositories.Interface;
using API.DTOs;
using API.Entities;
using API.Services.Interface;
using AutoMapper;

namespace API.Services
{
    public class InvoiceService(IInvoiceRepository invoiceRepository, ILogger<InvoiceService>logger, IMapper mapper) : IInvoiceService
    {
        private readonly IInvoiceRepository _invoiceRepository = invoiceRepository;
        private readonly ILogger<InvoiceService> _logger = logger;
        private readonly IMapper _mapper = mapper;
        public async Task<IEnumerable<InvoiceDto>> GetAllInvoicesAsync()
        {
            try
            {
                var invoices =  await _invoiceRepository.GetAllInvoicesAsync();
                // Map entities to DTOs
                return invoices.Select(i => _mapper.Map<InvoiceDto>(i)).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching all invoices.");
                throw;
            }
        }
        public async Task<Invoice> GetInvoiceAsync(int id)
        {
            try
            {
                return await _invoiceRepository.GetInvoiceAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching invoice with ID {InvoiceId}.", id);
                throw;
            }
        }
        public async Task<Invoice> AddInvoiceAsync(InvoiceDto invoiceDto)
        {
            try
            {
                invoiceDto.CreatedAt = DateTime.UtcNow;
                invoiceDto.UpdatedAt = DateTime.UtcNow;

                var invoice = _mapper.Map<Invoice>(invoiceDto);

                var createdInvoice = await _invoiceRepository.AddInvoiceAsync(invoice);
                return createdInvoice;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding a new invoice.");
                throw;
            }
        }
        public async Task UpdateInvoiceAsync(Invoice invoice)
        {
            try
            {
                await _invoiceRepository.UpdateInvoiceAsync(invoice);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating invoice with ID {InvoiceId}.", invoice.Id);
                throw;
            }
        }
        public async Task DeleteInvoiceAsync(int id)
        {
            try
            {
                await _invoiceRepository.DeleteInvoiceAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting invoice with ID {InvoiceId}.", id);
                throw;
            }
        }
    }
}

