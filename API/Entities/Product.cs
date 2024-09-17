using System.ComponentModel.DataAnnotations;

namespace API.Entities
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public string? Description { get; set; }
        public bool IsInStore {  get; set; }
        public string SKU { get; set; }
        public string Category { get; set; }
        public int StockQuantity { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string? ImageUrl { get; set; }
        public double? Weight { get; set; }
        public string? Dimensions { get; set; }
        public string? Manufacturer { get; set; }
        public double? Rating { get; set; }
        public bool? IsFeatured { get; set; }
        public List<OrderItem> OrderItems { get; set; }
    }
}

// Explanation 

// Product and OrderItem

    //Product: A product can appear in multiple order items.

    //OrderItem: Each order item refers to exactly one product.

//Relationship Type: One - to - Many

//Product (1) ---- (Many) OrderItem

//Explanation:

    //A Product can be part of many OrderItem records.

    //Each OrderItem references one Product.

//Product.cs:
//public class Product
//{
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
//    public int ProductId { get; set; }
//    public Product Product { get; set; }
//}