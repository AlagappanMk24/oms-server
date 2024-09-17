using API.Entities;
using System.ComponentModel.DataAnnotations;

namespace API.DTOs
{
    public class InvoiceDto
    {
        [Key]
        public int Id { get; set; }
        public string InvoiceNumber { get; set; }
        public string? PONumber { get; set; }
        public required DateTime InvoiceDue { get; set; }
        public required DateTime PaymentDue { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
        public List<InvoiceItemDto> InvoiceItems { get; set; }
        public List<InvoiceAttachmentsDto>? InvoiceAttachments { get; set; }
        public Customer? Customer {  get; set; } 
        public Location? Location { get; set; }
        public string? Notes { get; set; }
        public string? Status { get; set; }
        public string? InvoiceStatus { get; set; }

        [Required]
        public int CustomerId { get; set; }

        [Required]
        public int LocationId { get; set; }

        // Optional foreign key to Order
        public int? OrderId { get; set; }

        // Fields for invoice totals
        public decimal Subtotal { get; set; }
        public decimal Discount { get; set; }
        public decimal Tax { get; set; }
        public decimal TotalAmount { get; set; }
    }
    public class InvoiceItemDto
    {
        public int Id { get; set; }
        public required string Service { get; set; }
        public required string Description { get; set; }
        public required string Unit { get; set; }
        public required decimal Price { get; set; }
        public required decimal Amount { get; set; }
    }
    public class InvoiceAttachmentsDto
    {
        public int Id { get; set; }
        public string? AttachmentName { get; set; }
        public string? AttachmentContent { get; set; }
    }
}

// Sample Payload for Invoice

