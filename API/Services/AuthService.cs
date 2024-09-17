using API.DTOs;
using API.Services.Interface;
using AutoMapper.Internal;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using API.Data.Context;
using API.Data.Repositories.Interface;
using API.Model;
using Microsoft.EntityFrameworkCore;
using API.Entities;

namespace API.Services
{
    public class AuthService(AppDbContext dbContext, IHttpContextAccessor httpContextAccessor, IUserRepository userRepository, IEmailService emailService, IConfiguration configuration) : IAuthService
    {
        private readonly AppDbContext _dbContext = dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
        private readonly IUserRepository _userRepository = userRepository;
        private readonly IEmailService _emailService = emailService;
        private readonly IConfiguration _configuration = configuration;
        public bool RegisterUser(SignupDto registerModel)
        {
            // Check if a user with the provided email already exists
            var existingUser = _dbContext.Users.FirstOrDefault(u => u.Email == registerModel.Email);
            if (existingUser != null)
            {
                // User already exists, return false
                return false;
            }

            //Generate a random salt value
            byte[] salt = new byte[16];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            // Hash the password using PBKDF2 with a 128-bit salt, 10000 iterations, and SHA-256 hashing algorithm
            string hashedPassword = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: registerModel.Password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));

            // Create a new user entity with the registration data
            var user = new User
            {
                Username = registerModel.Username,
                Email = registerModel.Email,
                Password = hashedPassword,
                Salt = Convert.ToBase64String(salt),
                Role = null,
                DateCreated = DateTime.Now,
                DateModified = DateTime.Now,
                IsActive = true
            };

            // Add the user to the database
            _dbContext.Users.Add(user);
            _dbContext.SaveChanges();
            return true;
        }
        public User GetUserByEmail(string email)
        {
            // Fetch the user from the database based on the email address.
            return _dbContext.Users.FirstOrDefault(u => u.Email == email);
        }
        public async Task<bool> LoginUser(LoginDto loginModel)
        {
            var user = GetUserByEmail(loginModel.Email);

            if (user != null && VerifyPassword(user, loginModel.Password))
            {
                return true;
            }
            return false;
        }
        public async Task<string> GetUserRoleByEmailAsync(string email)
        {
            // Assuming you have a User table with an Email column and a Role column
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == email);

            // If user is found, return their role, otherwise return null or an empty string
            return user != null ? user.Role : null;
        }
        public async Task<AuthResponse> CreateUserIfNotExistsAsync(string email, string username, string authProvider)
        {
            var existingUser = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == email);

            if (existingUser == null)
            {
                var newUser = new User
                {
                    Username = username,
                    Email = email,
                    DateCreated = DateTime.Now,
                    DateModified = DateTime.Now,
                    IsActive = true,
                    IsExternalUser = true,
                    AuthProvider = authProvider
                };

                _dbContext.Users.Add(newUser);
                await _dbContext.SaveChangesAsync();
                return new AuthResponse { IsSuccessful = true, Message = "User created successfully" };
            }
            else
            {
                return new AuthResponse { IsSuccessful = true, Message = "User already exists" };
            }
        }
        private bool VerifyPassword(User user, string password)
        {
            byte[] salt = Convert.FromBase64String(user.Salt);
            string hashedPassword = Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));

            return user.Password == hashedPassword;
        }
        public VerifyOTPDto GenerateAndSendOTP(MailRequestDto model)
        {
            VerifyOTPDto viewModel = new VerifyOTPDto();

            string otp = GenerateOTP();

            // Generate token and get expiration time
            (string token, DateTime expiryTime) = CreateToken(model.ToEmail, otp);

            byte[] key = GenerateKey();
            byte[] iv = GenerateIV();

            // Encrypt the token
            string encryptedToken = Encrypt(token, key, iv);
            string encryptedKey = Convert.ToBase64String(key);
            string encryptedIV = Convert.ToBase64String(iv);

            model.OTP = otp;
            model.Token = encryptedToken;

            _emailService.SendEmail(model);

            viewModel.EncryptedToken = encryptedToken;
            viewModel.EncryptedKey = encryptedKey;
            viewModel.EncryptedIV = encryptedIV;
            viewModel.ToEmail = model.ToEmail;
            viewModel.OTP = model.OTP;
            viewModel.ExpiryTime = expiryTime;

            return viewModel;
        }
        private string GenerateOTP()
        {
            Random rnd = new Random();
            return rnd.Next(100000, 999999).ToString();
        }
        private (string token, DateTime expiryTime) CreateToken(string email, string otp)
        {
            DateTime expiryTime = DateTime.Now.AddMinutes(10);
            string token = $"{email}|{otp}|{expiryTime:o}";

            return (token, expiryTime);
        }
        private string Encrypt(string token, byte[] key, byte[] iv)
        {
            // Encrypt token using a symmetric encryption algorithm
            byte[] data = Encoding.UTF8.GetBytes(token);
            byte[] encryptedData;

            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = key;
                aesAlg.IV = iv;

                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);
                encryptedData = encryptor.TransformFinalBlock(data, 0, data.Length);
            }

            return Convert.ToBase64String(encryptedData);
        }
        private byte[] GenerateKey()
        {
            // Generate a random encryption key
            byte[] key = new byte[32]; // 256 bits
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(key);
            }
            return key;
        }
        private byte[] GenerateIV()
        {
            // Generate a random initialization vector (IV)
            byte[] iv = new byte[16]; // 128 bits
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(iv);
            }
            return iv;
        }
        public async Task<bool> SignInUserAfterOTP(string email)
        {
            var user = GetUserByEmail(email);

            if (user != null)
            {
                await SignInAsync(user);
                return true;
            }

            return false;
        }
        private async Task SignInAsync(User user)
        {
            var claims = await GenerateClaims(user);
            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties
            {
                AllowRefresh = true,
            };

            await _httpContextAccessor.HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);
        }
        private async Task<IEnumerable<Claim>> GenerateClaims(User user)
        {
            var userRole = await GetUserRoleByEmailAsync(user.Email);
            return new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Email),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, userRole)
            };
        }
        public User GetUserByToken(string token)
        {
            return _userRepository.GetUserByToken(token);
        }
        public string GeneratePasswordResetToken(User user)
        {
            // Generate a unique token
            var token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(32));

            // Store the token and its expiry time
            user.ResetToken = token;
            user.ResetTokenExpiry = DateTime.Now.AddHours(1); // Token valid for 1 hour

            _userRepository.UpdateUser(user);

            return token;
        }
        public bool ResetPassword(User user, string token, string newPassword)
        {
            // Update the user's password
            byte[] salt = Convert.FromBase64String(user.Salt);
            string hashedPassword = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: newPassword,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));

            user.Password = hashedPassword;
            user.ResetToken = null; // Clear the reset token
            user.ResetTokenExpiry = null; // Clear the reset token expiry

            _userRepository.UpdateUser(user);

            // Send email notification
            _emailService.SendPasswordResetConfirmation(user.Email);

            return true;
        }
    }
}
