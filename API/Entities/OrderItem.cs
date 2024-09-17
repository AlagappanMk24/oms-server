using System.ComponentModel.DataAnnotations;

namespace API.Entities
{
    // Relationship b/w order and product
    public class OrderItem
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int OrderId { get; set; }
        public Order? Order { get; set; } // Navigation Property for Order

        [Required]
        public int ProductId { get; set; }
        public Product? Product { get; set; } // Navigation Property for Product
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
        //public decimal TotalPrice => Quantity * UnitPrice;
    }
}
