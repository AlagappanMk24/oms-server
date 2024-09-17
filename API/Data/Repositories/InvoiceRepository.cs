using API.Data.Context;
using API.Data.Repositories.Interface;
using API.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Data.Repositories
{
    public class InvoiceRepository(AppDbContext context) : IInvoiceRepository
    {
        private readonly AppDbContext _context = context;
        public async Task<Invoice> GetInvoiceAsync(int id)
        {
            return await _context.Invoices
                .Include(i => i.InvoiceItems)
                .Include(a=>a.InvoiceAttachments)
                .Include(c => c.Customer)
                .FirstOrDefaultAsync(i => i.Id == id);
        }
        public async Task<IEnumerable<Invoice>> GetAllInvoicesAsync()
        {
            // return whole customer objects

            //return await _context.Invoices
            //    .Include(i => i.InvoiceItems)
            //    .Include(a=> a.InvoiceAttachments)
            //    .Include(c=>c.Customer)
            //    .Where(i=>i.Status == "Active")
            //    .OrderByDescending(i => i.CreatedAt)
            //    .ToListAsync();

            // return necessary fields

            return await _context.Invoices
               .AsNoTracking()
               .Include(i => i.InvoiceItems)
               .Include(a => a.InvoiceAttachments)
               .Include(c => c.Customer)
               .Include(l => l.Location)
                    .ThenInclude(l => l.Currency)
               .Where(i => i.Status == "Active")
               .OrderByDescending(i => i.CreatedAt)
               .Select(i => new Invoice
               {
                   Id = i.Id,
                   InvoiceNumber = i.InvoiceNumber,
                   PONumber = i.PONumber,
                   InvoiceDue = i.InvoiceDue,
                   PaymentDue = i.PaymentDue,
                   CreatedAt = i.CreatedAt,
                   UpdatedAt = i.UpdatedAt,
                   Status = i.Status,
                   InvoiceStatus = i.InvoiceStatus,
                   InvoiceItems = i.InvoiceItems,
                   InvoiceAttachments = i.InvoiceAttachments,
                   Notes = i.Notes,
                   CustomerId = i.CustomerId,
                   LocationId = i.LocationId,
                   OrderId = i.OrderId,
                   Subtotal = i.Subtotal,
                   Discount = i.Discount,
                   Tax = i.Tax,
                   TotalAmount = i.TotalAmount,
                   Customer = new Customer
                   {
                       Id = i.Customer.Id,
                       Name = i.Customer.Name,
                       Email = i.Customer.Email,
                       PhoneNumber = i.Customer.PhoneNumber,
                       Address = i.Customer.Address
                   },
                   Location = new Location
                   {
                       Id = i.Location.Id,
                       Currency = new Currency
                       {
                            Id = i.Location.Currency.Id,
                            Code = i.Location.Currency.Code,
                            Symbol = i.Location.Currency.Symbol,
                            Name = i.Location.Currency.Name
                       }
                   },
                   Order = i.Order
               })
               .ToListAsync();
        }
        public async Task<Invoice> AddInvoiceAsync(Invoice invoice)
        {
            await _context.Invoices.AddAsync(invoice);
            await _context.SaveChangesAsync();
            return await _context.Invoices
                .OrderByDescending(i => i.CreatedAt)
                .FirstOrDefaultAsync();
        }
        public async Task UpdateInvoiceAsync(Invoice invoice)
        {
            _context.Invoices.Update(invoice);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteInvoiceAsync(int id)
        {
            var invoice = await _context.Invoices.FindAsync(id);
            if (invoice != null)
            {
                _context.Invoices.Remove(invoice);
                await _context.SaveChangesAsync();
            }
        }
    }
}
