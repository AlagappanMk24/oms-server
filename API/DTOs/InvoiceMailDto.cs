using API.Entities;

namespace API.DTOs
{
    public class InvoiceMailDto
    {
        public required string InvoiceId { get; set; }
        public required string From { get; set; }
        public required string ToEmail { get; set; }
        public required string Subject { get; set; }
        public string? Body { get; set; }
        public string? Cc { get; set; }
        public bool IsAttachPdf { get; set; }
        public bool SendCopyToMyself { get; set; }
        public string? InvoiceNumber { get; set; }
        public DateTime InvoiceDue { get; set; }
        public string? Logo { get; set; }
        public decimal? TotalAmount { get; set; }
        public Currency? Currency { get; set; }
    }
}
