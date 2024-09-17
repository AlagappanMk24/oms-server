namespace API.DTOs
{
    public class ProductDto
    {
        public int Id { get; set;}
        public required string Name { get; set; }
        public required string Description { get; set; }
        public double Price { get; set; }
        public bool IsInStore { get; set; }
        public required string SKU { get; set; }
        public required string Category { get; set; }
        public int StockQuantity { get; set; }
        public required string ImageUrl { get; set; }
        public double? Weight { get; set; }
        public required string Dimensions { get; set; }
        public string? Manufacturer { get; set; }
        public double? Rating { get; set; }
        public bool? IsFeatured { get; set; }
    }

}

// Sample Payload

//{
//    "id": 1,
//    "name": "Wireless Mouse",
//    "description": "Ergonomic wireless mouse with USB receiver",
//    "price": 29.99,
//    "isInStore": true,
//    "SKU": "WM12345",
//    "category": "Electronics",
//    "stockQuantity": 150,
//    "imageUrl": "https://example.com/images/wireless-mouse.jpg",
//    "weight": 0.5,
//    "dimensions": "4.5 x 2.5 x 1.5 inches",
//    "manufacturer": "TechBrand",
//    "rating": 4.5,
//    "isFeatured": true
//}
