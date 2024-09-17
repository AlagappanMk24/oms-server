using System.ComponentModel.DataAnnotations;

namespace API.Entities
{
    public class Invoice
    {
        [Key]
        public int Id { get; set; }
        public string InvoiceNumber { get; set; }
        public string? PONumber { get; set; }
        public required DateTime InvoiceDue { get; set; }
        public required DateTime PaymentDue { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
        public string? Status { get; set; }
        public string? InvoiceStatus { get; set; }
        public required List<InvoiceItem> InvoiceItems { get; set; }

        // File Attachments
        public List<InvoiceAttachments>? InvoiceAttachments { get; set; }
        public string? Notes { get; set; }

        [Required]
        public int CustomerId { get; set; }
        public Customer? Customer { get; set; } // Navigation property

        [Required]
        public int LocationId { get; set; }
        public Location? Location { get; set; } // Navigation property

        // Optional foreign key to Order
        public int? OrderId { get; set; }
        public Order? Order { get; set; }

        // Fields for invoice totals
        public decimal Subtotal { get; set; }
        public decimal Discount { get; set; }
        public decimal Tax { get; set; }
        public decimal TotalAmount { get; set; }

    }
}


// Explanation : Invoice and InvoiceItem

// Invoice: An invoice can contain multiple invoice items.

// InvoiceItem: Each invoice item is associated with one invoice.

// Relationship Type: One - to - Many

// Invoice (1) ---- (Many) InvoiceItem

// Explanation:

// An Invoice can have multiple InvoiceItem records.

// Each InvoiceItem is linked to one Invoice.


// Invoice.cs:
// public class Invoice
// {
//    [Key]
//    public int Id { get; set; }
//    // Other properties...

//    public List<InvoiceItem> Items { get; set; }
// }

// InvoiceItem.cs:
// public class InvoiceItem
// {
//    [Key]
//    public int Id { get; set; }

//    [Required]
//    public int InvoiceId { get; set; }
//    public Invoice Invoice { get; set; }
// }

// Explanation : Order and Invoice

// Order: An order might be linked to one invoice.

// Invoice: An invoice can optionally be linked to an order.

// Relationship Type: One - to - One or Optional

// Order (1) ---- (0 or 1) Invoice

// Explanation:

//An Order can have zero or one Invoice.

//An Invoice might be associated with an Order, but it’s not mandatory.


// Order.cs:
// public class Order
// {
//    [Key]
//    public int Id { get; set; }
//    // Other properties...

//    public int? InvoiceId { get; set; }
//    public Invoice Invoice { get; set; }
// }

// Invoice.cs:
// public class Invoice
// {
//    [Key]
//    public int Id { get; set; }
//    // Other properties...

//    public int? OrderId { get; set; }
//    public Order Order { get; set; }
// }