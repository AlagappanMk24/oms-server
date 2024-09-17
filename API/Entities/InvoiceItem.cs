using System.ComponentModel.DataAnnotations;

namespace API.Entities
{
    public class InvoiceItem
    {
        public int Id { get; set; }
        public required string Service { get; set; }
        public required string Description { get; set; }
        public required string Unit { get; set; }
        public required decimal Price { get; set; }
        public required decimal Amount { get; set; }

        [Required]
        public int InvoiceId { get; set; }
        public Invoice Invoice { get; set; }
    }
}
