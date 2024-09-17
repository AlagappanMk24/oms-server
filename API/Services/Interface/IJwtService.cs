using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace API.Services.Interface
{
    public interface IJwtService
    {
        string GenerateJwtToken(string email, string otp);
        ClaimsPrincipal? ValidateJwtToken(string token, out SecurityToken? validatedToken);
        string GenerateToken(string username, string email);
    }
}
