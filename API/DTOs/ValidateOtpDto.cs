namespace API.DTOs
{
    public class ValidateOtpDto
    {
        public string OTP { get; set; }
        public DateTime ExpiryTime { get; set; }
        public bool IsResetPassword { get; set; }
    }
}
