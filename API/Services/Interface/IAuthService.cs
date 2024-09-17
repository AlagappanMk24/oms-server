using API.DTOs;
using API.Entities;
using API.Model;

namespace API.Services.Interface
{
    public interface IAuthService
    {
        bool RegisterUser(SignupDto registerModel);
        Task<bool> LoginUser(LoginDto loginModel);
        User GetUserByEmail(string email);
        VerifyOTPDto GenerateAndSendOTP(MailRequestDto model);
        Task<bool> SignInUserAfterOTP(string email);
        Task<AuthResponse> CreateUserIfNotExistsAsync(string email, string username, string authProvider);
        Task<string> GetUserRoleByEmailAsync(string email);
        User GetUserByToken(string token);
        string GeneratePasswordResetToken(User user);
        bool ResetPassword(User user, string token, string newPassword);
    }
}
