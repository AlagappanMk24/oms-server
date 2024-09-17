using System.ComponentModel.DataAnnotations;

namespace API.Entities
{
    public class InvoiceAttachments
    {
        [Key]
        public int Id { get; set; }
        public string? AttachmentName { get; set; }
        public string? AttachmentContent { get; set; }

        [Required]
        public int InvoiceId { get; set; }
        public Invoice? Invoice { get; set; }
    }
}
