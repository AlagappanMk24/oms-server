using System.ComponentModel.DataAnnotations;

namespace API.Entities
{
    public class Customer
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public Address Address { get; set; }

        // Navigation property for orders
        public List<Order>? Orders { get; set; }

        // Navigation property for invoices
        public List<Invoice>? Invoices { get; set; }
    }
}


// Explanation : Customer and Invoice

    // Customer: A customer can have multiple invoices.

    // Invoice: Each invoice is associated with one customer.

// Relationship Type: One - to - Many

// Customer (1) ---- (Many) Invoice

// Explanation:

    // A Customer can have multiple Invoice records.

    // Each Invoice is linked to one Customer.

// Customer.cs:

// public class Customer
//{
//    [Key]
//    public int Id { get; set; }
//    // Other properties...

//    public List<Invoice> Invoices { get; set; }
//}

// Invoice.cs:

// public class Invoice
// {
//    [Key]
//    public int Id { get; set; }
//    // Other properties...

//    [Required]
//    public int CustomerId { get; set; }
//    public Customer Customer { get; set; }
// }