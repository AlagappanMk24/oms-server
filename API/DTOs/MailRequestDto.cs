namespace API.DTOs
{
    public class MailRequestDto
    {
        public string? ToEmail { get; set; }
        public string? OTP { get; set; }
        public DateTime ExpiryTime { get; set; }
        public string? Token { get; set; }
        public string? ResetLink { get; set; }
        public bool? IsResetPassword { get; set; }
    }
}
