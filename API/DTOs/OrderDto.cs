using System.ComponentModel.DataAnnotations;

namespace API.DTOs
{
    public class OrderDto
    {
        public int Id { get; set; }
        public required string OrderNumber { get; set; }
        public DateTime OrderDate { get; set; }
        public required string OrderStatus { get; set; }
        public required AddressDto ShippingAddress { get; set; }
        public required AddressDto BillingAddress { get; set; }
        public required string PaymentMethod { get; set; }
        public required string PaymentStatus { get; set; }
        public decimal ShippingCost { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal Discount { get; set; }
        public decimal TotalAmount { get; set; }
        public string? Notes { get; set; }

        [Required]
        public int CustomerId { get; set; }   // Foreign key to Customer
        public List<OrderItemDto> OrderItems { get; set; }
    }
    public class AddressDto
    {
        public required string Address1 { get; set; }
        public required string Address2 { get; set; }
        public required string City { get; set; }
        public required string State { get; set; }
        public required string Country { get; set; }
        public required string ZipCode { get; set; }
    }

    public class OrderItemDto
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
    }
}


// Sample Payload

//{
//    "id": 1,
//    "orderNumber": "ORD123456",
//    "orderDate": "2023-07-31T00:00:00Z",
//    "orderStatus": "Processing",
//    "shippingAddress": {
//        "address1": "123 Main St",
//        "address2": "Apt 4B",
//        "city": "New York",
//        "state": "NY",
//        "country": "USA",
//        "zipCode": "10001"
//    },
//    "billingAddress": {
//        "address1": "123 Main St",
//        "address2": "Apt 4B",
//        "city": "New York",
//        "state": "NY",
//        "country": "USA",
//        "zipCode": "10001"
//    },
//    "paymentMethod": "Credit Card",
//    "paymentStatus": "Paid",
//    "shippingCost": 5.99,
//    "taxAmount": 2.50,
//    "discount": 3.00,
//    "totalAmount": 75.49,
//    "notes": "Leave package at the front door.",
//    "customerId": 1,
//    "orderItems": [
//        {
//        "id": 1,
//            "productId": 1,
//            "quantity": 2,
//            "unitPrice": 29.99,
//            "totalPrice": 59.98
//        },
//        {
//        "id": 2,
//            "productId": 2,
//            "quantity": 1,
//            "unitPrice": 18.99,
//            "totalPrice": 18.99
//        }
//    ]
//}
