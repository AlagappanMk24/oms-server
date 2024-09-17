using System.ComponentModel.DataAnnotations;

namespace API.Entities
{
    public class User
    {
        [Key]
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string? Password { get; set; }
        public string? Salt { get; set; }
        public string? Role { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        public bool IsActive { get; set; }
        public string? ResetToken { get; set; }
        public DateTime? ResetTokenExpiry { get; set; }
        public bool IsExternalUser { get; set; }
        public string? AuthProvider { get; set; }
    }
}
