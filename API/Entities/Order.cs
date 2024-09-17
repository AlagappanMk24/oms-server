using System.ComponentModel.DataAnnotations;

namespace API.Entities
{
    public class Order
    {
        [Key]
        public int Id { get; set; }
        public string OrderNumber { get; set; }
        public DateTime OrderDate { get; set; }
        public string OrderStatus { get; set; }
        public Address ShippingAddress { get; set; }  
        public Address BillingAddress { get; set; }  
        public string PaymentMethod { get; set; }
        public string PaymentStatus { get; set; }
        public decimal ShippingCost { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal Discount { get; set; }
        public decimal TotalAmount { get; set; }
        public string? Notes { get; set; }
        public int CustomerId { get; set; }   // Foreign key to Customer
        public Customer Customer { get; set; }// Navigation property for customer
        public List<OrderItem> OrderItems { get; set; } = new List<OrderItem>(); // Navigation property for order items

        // Optional foreign key to Invoice
        public int? InvoiceId { get; set; }
        public Invoice Invoice { get; set; }
    }

    public class Address
    {
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string ZipCode { get; set; }
    }
}

// Explanation : Customer and Order

// Customer: A customer can place multiple orders.

// Order: Each order is placed by exactly one customer.

// Relationship Type: One - to - Many

//Customer (1) ---- (Many) Order

// Explanation:

// A Customer can have multiple Order records.

// Each Order has one Customer.

// Customer.cs

//public class Customer
//{
//    [Key]
//    public int Id { get; set; }
//    // Other properties...

//    // Navigation property
//    public List<Order> Orders { get; set; }
//}

// Order.cs

//public class Order
//{
//    [Key]
//    public int Id { get; set; }
//    // Other properties...

//    [Required]
//    public int CustomerId { get; set; }
//    public Customer Customer { get; set; }
//}

// Explanation : Order and OrderItem

    // Order: An order can contain multiple order items.

    // OrderItem: Each order item is part of one specific order.

// Relationship Type: One - to - Many

// Order (1) ---- (Many) OrderItem

// Explanation:

    // An Order can have multiple OrderItem records.

    // Each OrderItem belongs to exactly one Order.


//Order.cs:
//public class Order
//{
//    [Key]
//    public int Id { get; set; }
//    // Other properties...

//    public List<OrderItem> OrderItems { get; set; }
//}

//OrderItem.cs:

//public class OrderItem
//{
//    [Key]
//    public int Id { get; set; }

//    [Required]
//    public int OrderId { get; set; }
//    public Order Order { get; set; }
//}
