namespace API.DTOs
{
    public class VerifyOTPDto
    {
        public string? EncryptedToken { get; set; }
        public string? EncryptedKey { get; set; }
        public string? EncryptedIV { get; set; }
        public string? ToEmail { get; set; }
        public string? Message { get; set; }
        public string? MessageClass { get; set; }
        public string? OTP { get; set; }
        public DateTime ExpiryTime { get; set; }
    }
}
