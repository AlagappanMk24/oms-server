using API.DTOs;
using API.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(IAuthService authService, ILogger<AuthController> logger, IJwtService jwtService, IEmailService emailService) : ControllerBase
    {
        private readonly IAuthService _authService = authService;
        private readonly ILogger<AuthController> _logger = logger;
        private readonly IJwtService _jwtService = jwtService;
        private readonly IEmailService _emailService = emailService;

        [HttpPost("/SignUp")]
        public IActionResult SignUp(SignupDto signUpDto)
        {
            _logger.LogInformation("User registration attempt for email: {Email}", signUpDto.Email);
            try
            {
                if (ModelState.IsValid)
                {
                    var isRegistered = _authService.RegisterUser(signUpDto);
                    if (isRegistered)
                    {
                        _logger.LogInformation("User registered successfully: {Email}", signUpDto.Email);
                        return Ok(new { status = "success", message = "User Registered Successfully" });
                    }
                    else
                    {
                        _logger.LogWarning("Registration failed. User with email {Email} already exists.", signUpDto.Email);
                        return Ok(new { status = "error", message = "User with this email already exists." });
                    }
                }
                return BadRequest(signUpDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred during registration for email: {Email}", signUpDto.Email);
                return StatusCode(500, "An error occurred while processing the request.");
            }
        }

        [HttpPost("/Login")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            _logger.LogInformation("User login attempt for email: {Email}", loginDto.Email);
            try
            {
                if (ModelState.IsValid)
                {
                    var isAuthenticated = await _authService.LoginUser(loginDto);
                    if (isAuthenticated)
                    {
                        var user = _authService.GetUserByEmail(loginDto.Email);
                        var token = _jwtService.GenerateToken(user.Username, user.Email);
                        _logger.LogInformation("User logged in successfully: {Email} (User: {Username})", loginDto.Email, user.Username);
                        return Ok(new { status = "success", email = loginDto.Email, generatedToken = token });
                    }
                    _logger.LogWarning("Login failed for email: {Email}. Invalid email or password.", loginDto.Email);
                    return BadRequest(new { status = "error", message = "Invalid email or password." });
                }
                return BadRequest(new { status = "error", message = "Invalid model state." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred during login for email: {Email}", loginDto.Email);
                return StatusCode(500, "An error occurred while processing the request.");
            }
        }

        [HttpPost("/SendOTP")]
        public IActionResult SendOTP(MailRequestDto model)
        {
            _logger.LogInformation("OTP verification attempt for email: {Email}", model.ToEmail);
            try
            {
                var response = _authService.GenerateAndSendOTP(model);

                // Generate JWT containing OTP
                var jwtToken = _jwtService.GenerateJwtToken(model.ToEmail, response.OTP);

                _logger.LogInformation("OTP sent successfully to email: {Email}", model.ToEmail);

                // Return JWT token to frontend
                return Ok(new { jwtToken, status = "success", expiryTime = response.ExpiryTime, emailAddress = model.ToEmail });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred during OTP verification for email: {Email}", model.ToEmail);
                return StatusCode(500, "An error occurred while processing the request.");
            }
        }

        [HttpPost("/ValidateOTP")]
        public IActionResult ValidateOTP([FromHeader(Name = "Authorization")] string authorizationHeader, ValidateOtpDto validateOTPDto)
        {
            // Extract JWT token from Authorization header
            if (string.IsNullOrEmpty(authorizationHeader) || !authorizationHeader.StartsWith("Bearer "))
            {
                return Unauthorized("Invalid authorization header");
            }

            string jwtToken = authorizationHeader.Substring("Bearer ".Length).Trim();

            // Validate JWT token and extract OTP
            var claimsPrincipal = _jwtService.ValidateJwtToken(jwtToken, out SecurityToken validatedToken);
            if (claimsPrincipal == null)
            {
                if (validatedToken == null)
                {
                    return Conflict(new { status = "error", message = "OTP has expired" });
                }
                return BadRequest(new { status = "error", message = "Invalid OTP." });
            }

            // Extract email and OTP from claims
            var emailClaim = claimsPrincipal.FindFirst("email");
            var otpClaim = claimsPrincipal.FindFirst("otp");

            if (emailClaim == null || otpClaim == null)
            {
                return BadRequest(new { status = "error", message = "Invalid OTP." });
            }

            string email = emailClaim.Value;
            string tokenOtp = otpClaim.Value;

            // Compare tokenOtp with otp
            if (tokenOtp != validateOTPDto.OTP)
            {
                return BadRequest(new { status = "error", message = "Invalid OTP.", emailAddress = email });
            }

            // Check if token is expired
            var expiryTime = validateOTPDto.ExpiryTime;

            if (DateTime.Now > expiryTime)
            {
                return Conflict(new { status = "error", message = "OTP has expired", emailAddress = email });
            }

            // OTP validation success
            return Ok(new { success = true, message = "OTP verified successfully", emailAddress = email });
        }

        [HttpPost("/ForgotPassword")]
        public IActionResult ForgotPassword([FromBody] ForgotPasswordDto model)
        {
            var user = _authService.GetUserByEmail(model.Email);
            if (user == null)
            {
                return BadRequest(new { status = "error", message = "User not found." });
            }
            var token = _authService.GeneratePasswordResetToken(user);

            // Send email with the reset link
            _emailService.SendResetPasswordLink(token, user.Email);

            return Ok(new { status = "success", resetToken = token, message = "A password reset link has been sent to your email. Please check your inbox." });
        }

        [HttpPost("/ResetPassword")]
        public IActionResult ResetPassword([FromBody] ResetPasswordDto model)
        {
            var user = _authService.GetUserByEmail(model.Email);
            if (user == null)
            {
                return BadRequest(new { status = "error", message = "User not found." });
            }
            if (user.ResetTokenExpiry < DateTime.Now)
            {
                return BadRequest(new { status = "error", message = "Token has expired." });
            }
            var result = _authService.ResetPassword(user, model.Token, model.Password);
            if (result)
            {
                return Ok(new { status = "success", message = "Password has been reset successfully." });
            }
            return BadRequest(new { status = "error", message = "Failed to reset password." });
        }

        [HttpPost("/Logout")]
        public IActionResult Logout()
        {
            _logger.LogInformation("User logout attempt");

            try
            {
                // Perform logout actions here (e.g., invalidate tokens, clear cookies)
                // For this example, we'll simply log the logout action.
                // If using a token blacklist, you would add the token to the blacklist here.

                return Ok(new { status = "success", message = "User logged out successfully." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred during logout.");
                return StatusCode(500, "An error occurred while processing the request.");
            }
        }

        //[HttpPost("/GoogleLogin")]
        //public async Task<IActionResult> GoogleLogin([FromBody] GoogleLoginDto googleLoginDto)
        //{
        //    try
        //    {
        //        var payload = await _authService.VerifyGoogleToken(googleLoginDto);
        //        if (payload == null)
        //        {
        //            return BadRequest(new { status = "error", message = "Invalid Google token." });
        //        }
        //        var user = _authService.GetOrCreateUser(payload);
        //        var token = _jwtService.GenerateToken(user.Username, user.Email);

        //        return Ok(new { status = "success", email = user.Email, generatedToken = token });
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "An error occurred during Google login.");
        //        return StatusCode(500, "An error occurred while processing the request.");
        //    }
        //}
    }
}
