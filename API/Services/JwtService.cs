using API.Services.Interface;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace API.Services
{
    public class JwtService(string secretKey) : IJwtService
    {
        private readonly string _secretKey = secretKey;

        // Method to generate a JWT token based on email and OTP
        public string GenerateJwtToken(string email, string otp)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            // Define claims for the token
            var claims = new[]
            {
                new Claim("email", email),
                new Claim("otp", otp)
            };

            // Create a JWT token with specified claims, expiry time, and signing credentials
            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(15),// Example: token expiry time (15 minutes from now)
                signingCredentials: credentials
            );

            // Write and return the JWT token as a string
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        // Method to validate a JWT token and return the ClaimsPrincipal
        public ClaimsPrincipal? ValidateJwtToken(string token, out SecurityToken? validatedToken)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_secretKey);

            try
            {
                // Validate the provided token using the secret key and specified validation parameters
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                }, out validatedToken);

                // Create a ClaimsPrincipal from the validated token and return it
                var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity((validatedToken as JwtSecurityToken)?.Claims));
                return claimsPrincipal;
            }
            catch (SecurityTokenExpiredException)
            {
                validatedToken = null;
                return null; // Return null to indicate the token has expired
            }
            catch (Exception)
            {
                validatedToken = null;
                return null; // Return null to indicate an invalid token
            }
        }

        // Method to generate a token based on username and email
        public string GenerateToken(string username, string email)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_secretKey);

            // Define token descriptor with claims, expiry time, and signing credentials
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, username),
                    new Claim(ClaimTypes.Email, email)
                }),
                Expires = DateTime.UtcNow.AddHours(1),// Example: token expiry time (1 hour from now)
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            // Create and return the token as a string
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
