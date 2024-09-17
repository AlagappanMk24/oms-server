using API.DTOs;
using API.Entities;
using API.Services;
using API.Services.Interface;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class InvoiceController(IInvoiceService invoiceService, IEmailService emailService, IPdfService pdfService, ILocationService locationService, IMapper mapper, ILogger<InvoiceController> logger) : ControllerBase
    {
        private readonly IInvoiceService _invoiceService = invoiceService;
        private readonly IEmailService _emailService = emailService;
        private readonly IPdfService _pdfService = pdfService;
        private readonly ILocationService _locationService = locationService;
        private readonly IMapper _mapper = mapper;
        private readonly ILogger<InvoiceController> _logger = logger;

        [HttpGet]
        public async Task<IActionResult> GetAllInvoices()
        {
            try
            {
                _logger.LogInformation("Fetching all invoices.");

                var invoices = await _invoiceService.GetAllInvoicesAsync();

                if (invoices == null)
                {
                    _logger.LogWarning("No invoices found.");
                    return NotFound("No invoices found.");
                }

                _logger.LogInformation("Successfully fetched all invoices.");

                return Ok(invoices);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching all invoices.");

                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetInvoice(int id)
        {
            try
            {
                _logger.LogInformation("Fetching invoice with ID {InvoiceId}.", id);

                var invoice = await _invoiceService.GetInvoiceAsync(id);

                if (invoice == null)
                {
                    _logger.LogWarning("Invoice with ID {InvoiceId} not found.", id);

                    return NotFound();
                }

                _logger.LogInformation("Successfully fetched invoice with ID {InvoiceId}.", id);

                return Ok(invoice);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching invoice with ID {InvoiceId}.", id);

                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateInvoice(InvoiceDto invoiceDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("Invalid model state for creating invoice.");

                    return BadRequest(ModelState);
                }

                if (invoiceDto == null)
                {
                    _logger.LogWarning("Invoice is null.");

                    return BadRequest();
                }

                _logger.LogInformation("Creating a new invoice.");

                var createdInvoice = await _invoiceService.AddInvoiceAsync(invoiceDto);

                _logger.LogInformation("Successfully created invoice with ID {InvoiceId}.", createdInvoice.Id);

                return CreatedAtAction(nameof(GetInvoice), new { id = createdInvoice.Id }, createdInvoice);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateInvoice(int id, [FromBody] Invoice invoice)
        {
            try
            {
                if (invoice == null || invoice.Id != id)
                {
                    _logger.LogWarning("Invalid invoice update request. Invoice ID mismatch or invoice is null.");

                    return BadRequest();
                }

                _logger.LogInformation("Updating invoice with ID {InvoiceId}.", id);

                var existingInvoice = await _invoiceService.GetInvoiceAsync(id);

                if (existingInvoice == null)
                {
                    _logger.LogWarning("Invoice with ID {InvoiceId} not found.", id);

                    return NotFound();
                }

                await _invoiceService.UpdateInvoiceAsync(invoice);

                _logger.LogInformation("Successfully updated invoice with ID {InvoiceId}.", id);

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating invoice with ID {InvoiceId}.", id);

                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteInvoice(int id)
        {
            try
            {
                _logger.LogInformation("Deleting invoice with ID {InvoiceId}.", id);

                var invoice = await _invoiceService.GetInvoiceAsync(id);

                if (invoice == null)
                {
                    _logger.LogWarning("Invoice with ID {InvoiceId} not found.", id);

                    return NotFound();
                }

                await _invoiceService.DeleteInvoiceAsync(id);

                _logger.LogInformation("Successfully deleted invoice with ID {InvoiceId}.", id);

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting invoice with ID {InvoiceId}.", id);

                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("email")]
        public async Task<IActionResult> SendInvoiceToEmail(InvoiceMailDto invoiceMailDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                byte[]? pdfContent = null;
                Invoice? invoiceDetails = null;

                // Fetch the invoice details if an ID is provided
                if (invoiceMailDto.IsAttachPdf && int.TryParse(invoiceMailDto.InvoiceId, out int invoiceId))
                {
                    invoiceDetails = await _invoiceService.GetInvoiceAsync(invoiceId);

                    if (invoiceDetails == null)
                    {
                        return NotFound("Invoice not found.");
                    }

                    var locationDetails = await _locationService.GetLocationByIdAsync(invoiceDetails.LocationId);

                    if (locationDetails == null)
                    {
                        return NotFound("Location not found.");
                    }

                    // Initialize the InvoicePdfDto and its Location and Currency properties
                    var invoicePdfData = new Invoice
                    {
                        InvoiceNumber = invoiceDetails.InvoiceNumber,
                        PONumber = invoiceDetails.PONumber,
                        InvoiceDue = invoiceDetails.InvoiceDue,
                        PaymentDue = invoiceDetails.PaymentDue,
                        CreatedAt = invoiceDetails.CreatedAt,
                        UpdatedAt = invoiceDetails.UpdatedAt,
                        InvoiceItems = invoiceDetails.InvoiceItems,
                        Notes = invoiceDetails.Notes,
                        CustomerId = invoiceDetails.CustomerId,
                        Customer = invoiceDetails.Customer,
                        LocationId = invoiceDetails.LocationId,
                        Location = new Location
                        {
                            Logo = locationDetails.Logo,
                            Currency = new Currency
                            {
                                Id = locationDetails.Currency.Id,
                                Code = locationDetails.Currency.Code,
                                Symbol = locationDetails.Currency.Symbol,
                                Name = locationDetails.Currency.Name
                            },
                            Address = locationDetails.Address,
                            City = locationDetails.City,
                            State = locationDetails.State,
                            Country = locationDetails.Country,
                            PostalCode = locationDetails.PostalCode
                        },
                        OrderId = invoiceDetails.OrderId,
                        Order = invoiceDetails.Order,
                        Subtotal = invoiceDetails.Subtotal,
                        Discount = invoiceDetails.Discount,
                        Tax = invoiceDetails.Tax,
                        TotalAmount = invoiceDetails.TotalAmount,
                        InvoiceAttachments = invoiceDetails.InvoiceAttachments
                    };

                    // Generate PDF if invoice details are present
                    pdfContent = _pdfService.GenerateInvoicePdf(invoicePdfData);
                }
                else if (invoiceMailDto.IsAttachPdf)
                {
                    return BadRequest("Invalid Invoice ID.");
                }

                // Populate invoice details in the DTO if an invoice was found
                if (invoiceDetails != null)
                {
                    invoiceMailDto.Logo = invoiceDetails.Location.Logo;
                    invoiceMailDto.Currency = new Currency
                    {
                        Id = invoiceDetails.Location.Currency.Id,
                        Code = invoiceDetails.Location.Currency.Code,
                        Symbol = invoiceDetails.Location.Currency.Symbol,
                        Name = invoiceDetails.Location.Currency.Name
                    };
                    invoiceMailDto.InvoiceNumber = invoiceDetails.InvoiceNumber;
                    invoiceMailDto.InvoiceDue = invoiceDetails.InvoiceDue;
                    invoiceMailDto.TotalAmount = invoiceDetails.TotalAmount;
                }
                // Send the email with the PDF attached
                _emailService.SendInvoiceToEmail(invoiceMailDto, pdfContent);

                _logger.LogInformation("Email sent successfully.");
                
                await UpdateInvoiceStatus(invoiceDetails.Id, "Sent");

                return Ok("Email sent successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while sending the email.");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while sending the email.");
            }
        }

        [HttpPost("generate-pdf/{invoiceId}")]
        public async Task<IActionResult> GenerateInvoicePdf(int invoiceId)
        {
            var invoice = await _invoiceService.GetInvoiceAsync(invoiceId);
            if (invoice == null)
            {
                return NotFound("Invoice not found");
            }
            var locationDetails = await _locationService.GetLocationByIdAsync(invoice.LocationId);

            // Initialize the InvoicePdfDto and its Location and Currency properties
            var invoicePdfData = new Invoice
            {
                InvoiceNumber = invoice.InvoiceNumber,
                PONumber = invoice.PONumber,
                InvoiceDue = invoice.InvoiceDue,
                PaymentDue = invoice.PaymentDue,
                CreatedAt = invoice.CreatedAt,
                UpdatedAt = invoice.UpdatedAt,
                InvoiceItems = invoice.InvoiceItems,
                Notes = invoice.Notes,
                CustomerId = invoice.CustomerId,
                Customer = invoice.Customer,
                LocationId = invoice.LocationId,
                Location = invoice.Location,
                OrderId = invoice.OrderId,
                Order = invoice.Order,
                Subtotal = invoice.Subtotal,
                Discount = invoice.Discount,
                Tax = invoice.Tax,
                TotalAmount = invoice.TotalAmount,
                InvoiceAttachments = invoice.InvoiceAttachments,
            };

            // If location details exist, populate the Location details in the DTO
            if (locationDetails != null)
            {
                invoicePdfData.Location.Logo = locationDetails.Logo;
                invoicePdfData.Location.Currency = new Currency
                {
                    Id = locationDetails.Currency.Id,
                    Code = locationDetails.Currency.Code,
                    Symbol = locationDetails.Currency.Symbol,
                    Name = locationDetails.Currency.Name
                };
                invoicePdfData.Location.Address = locationDetails.Address;
                invoicePdfData.Location.City = locationDetails.City;
                invoicePdfData.Location.State = locationDetails.State;
                invoicePdfData.Location.Country = locationDetails.Country;
                invoicePdfData.Location.PostalCode = locationDetails.PostalCode;
            }
            var pdfBytes = _pdfService.GenerateInvoicePdf(invoicePdfData);

            return File(pdfBytes, "application/pdf", "invoice.pdf");
        }

        [HttpPatch("{id}/status")]
        public async Task<IActionResult> UpdateInvoiceStatus(int id, [FromBody] string newStatus)
        {
            try
            {
                var invoice = await _invoiceService.GetInvoiceAsync(id);

                if (invoice == null)
                {
                    return NotFound("Invoice not found.");
                }

                invoice.InvoiceStatus = newStatus;
                await _invoiceService.UpdateInvoiceAsync(invoice);

                return Ok($"Invoice status updated to {newStatus}.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating invoice status.");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
