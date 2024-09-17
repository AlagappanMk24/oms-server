using API.DTOs;
using API.Services.Interface;
using API.Utilities.Email;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;

namespace API.Services
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings emailSettings;
        private readonly IInvoiceService _invoiceService;
        private readonly IPdfService _pdfService;
        public EmailService(IOptions<EmailSettings> options, IInvoiceService invoiceService, IPdfService pdfService)
        {
            this.emailSettings = options.Value;
            _invoiceService = invoiceService;
            _pdfService = pdfService;
        }
        public void SendEmail(MailRequestDto mailrequest)
        {
            var email = new MimeMessage();
            email.Sender = MailboxAddress.Parse(emailSettings.Email);
            email.To.Add(MailboxAddress.Parse(mailrequest.ToEmail));
            var builder = new BodyBuilder();

            if (mailrequest.OTP != null)
            {
                email.Subject = "Login OTP";
                builder.HtmlBody = GetOtpEmailBody(mailrequest.OTP);
            }
            else
            {
                email.Subject = "User Account Created";
                builder.HtmlBody = GetAccountCreatedEmailBody(mailrequest.ResetLink);
            }

            email.Body = builder.ToMessageBody();

            using var smtp = new SmtpClient();
            smtp.Connect(emailSettings.Host, emailSettings.Port, SecureSocketOptions.StartTls);
            smtp.Authenticate(emailSettings.Email, emailSettings.Password);
            smtp.Send(email);
            smtp.Disconnect(true);
        }
        public void SendInvoiceToEmail(InvoiceMailDto invoiceMailDto, byte[] pdfContent = null)
        {
            // Create a new MimeMessage
            var mailMessage = new MimeMessage
            {
                From = { MailboxAddress.Parse(invoiceMailDto.From) },
                To = { MailboxAddress.Parse(invoiceMailDto.ToEmail) }
            };

            // Add CC if provided
            if (!string.IsNullOrEmpty(invoiceMailDto.Cc))
            {
                mailMessage.Cc.Add(MailboxAddress.Parse(invoiceMailDto.Cc));
            }

            // Include 'From' address in CC if 'SendCopyToMyself' is true
            if (invoiceMailDto.SendCopyToMyself)
            {
                mailMessage.Cc.Add(MailboxAddress.Parse(invoiceMailDto.From));
            }

            // Set subject if provided
            if (!string.IsNullOrEmpty(invoiceMailDto.Subject))
            {
                mailMessage.Subject = invoiceMailDto.Subject; // Corrected line
            }

            // Build the email body
            var builder = new BodyBuilder
            {
                HtmlBody = $@"
        <!DOCTYPE html>
        <html lang='en' style='margin: 0; padding: 0; box-sizing: border-box; font-family: 'Arial', sans-serif;'>
        <head>
            <meta charset='UTF-8'>
            <meta name='viewport' content='width=device-width, initial-scale=1.0'>
            <title>Invoice</title>
            <style>
                body {{
                    margin: 0;
                    padding: 0;
                    background-color: #f0f4f8;
                    color: #333;
                }}
                .container {{
                    max-width: 600px;
                    margin: 20px auto;
                    padding: 20px;
                    background-color: #ffffff;
                    border-radius: 8px;
                    box-shadow: 0 0 15px rgba(0, 0, 0, 0.1);
                }}
                .header {{
                    text-align: center;
                    margin-bottom: 20px;
                    background-color: #007bff;
                    color: #ffffff;
                    padding: 20px;
                    border-radius: 8px 8px 0 0;
                }}
                .header img {{
                    max-width: 80px;
                    margin-bottom: 10px;
                }}
                .header h1 {{
                    font-size: 28px;
                    margin: 0;
                }}
                .content {{
                    margin: 20px 0;
                    color: #333333;
                }}
                .content p {{
                    font-size: 16px;
                    line-height: 1.7;
                    margin: 10px 0;
                }}
                .content ul {{
                    list-style: none;
                    padding: 0;
                }}
                .content ul li {{
                    background-color: #f7f7f7;
                    margin-bottom: 10px;
                    padding: 10px;
                    border-radius: 5px;
                }}
                .content ul li strong {{
                    color: #007bff;
                }}
                .footer {{
                    text-align: center;
                    font-size: 14px;
                    color: #777;
                    margin-top: 20px;
                }}
                .footer a {{
                    color: #007bff;
                    text-decoration: none;
                }}
                @media screen and (max-width: 600px) {{
                    .container {{
                        padding: 15px;
                    }}
                }}
            </style>
        </head>
        <body>
            <div class='container'>
                <div class='header'>
                    <img src={invoiceMailDto.Logo} alt='Company Logo'>
                    <h1>Invoice #{invoiceMailDto?.InvoiceNumber}</h1>
                </div>
                <div class='content'>
                    <p>Dear {invoiceMailDto?.ToEmail},</p>
                    <span>{invoiceMailDto?.Body}</span>
                    <p>Thank you for your business! Please find attached the invoice for your recent purchase. The details are as follows:</p>
                    <ul>
                        <li><strong>Invoice Number:</strong> {invoiceMailDto?.InvoiceNumber}</li>
                        <li><strong>Invoice Date:</strong> {invoiceMailDto?.InvoiceDue.ToString("MMMM dd, yyyy")}</li>
                        <li><strong>Total Amount:</strong> {invoiceMailDto?.Currency?.Symbol}{invoiceMailDto?.TotalAmount}</li>
                    </ul>
                    <p>If you have any questions or need further assistance, please don't hesitate to contact us. We appreciate your prompt payment.</p>
                    <p>Best regards,</p>
                    <p>{invoiceMailDto?.From}</p>
                </div>
                <div class='footer'>
                    <p>&copy; {DateTime.Now.Year} KLTINFOTECH. All rights reserved.</p>
                    <p><a href='{{UnsubscribeLink}}'>Unsubscribe</a></p>
                </div>
            </div>
        </body>
        </html>"
            };

            // Attach the PDF if provided
            if (pdfContent != null && invoiceMailDto.IsAttachPdf)
            {
                builder.Attachments.Add("invoice.pdf", pdfContent, ContentType.Parse("application/pdf"));
            }
            // Set the email body
            mailMessage.Body = builder.ToMessageBody();

            // Send the email
            using var smtp = new SmtpClient();
            try
            {
                smtp.Connect(emailSettings.Host, emailSettings.Port, SecureSocketOptions.StartTls);
                smtp.Authenticate(emailSettings.Email, emailSettings.Password);
                smtp.Send(mailMessage);
            }
            catch (Exception ex)
            {
                // Log or handle the exception as needed
                Console.WriteLine($"Error sending email: {ex.Message}");
            }
            finally
            {
                smtp.Disconnect(true);
            }
        }
        private async Task<Stream> DownloadFileAsync(string url)
        {
            using var httpClient = new HttpClient();
            var response = await httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStreamAsync();
        }
        public void SendResetPasswordLink(string token, string email)
        {
            var resetPasswordLink = $"http://localhost:3000/resetPassword?token={token}&email={email}";
            var message = new MimeMessage();
            message.Sender = MailboxAddress.Parse(emailSettings.Email);
            message.To.Add(MailboxAddress.Parse(email));
            message.Subject = "Password Reset";
            var builder = new BodyBuilder();

            // Include the reset password link in the email body with better styling
            builder.HtmlBody = $@"
            <html>
            <head>
                <style>
                    .email-container {{
                        font-family: Arial, sans-serif;
                        padding: 20px;
                        max-width: 600px;
                        background-color: #f9f9f9;
                        border-radius: 8px;
                        box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
                    }}
                    .email-header {{
                        background-color: #007bff;
                        color: #ffffff;
                        padding: 10px;
                        border-radius: 8px 8px 0 0;
                        text-align: center;
                    }}
                    .email-body {{
                        padding: 20px;
                    }}
                    .email-body p {{
                        line-height: 1.6;
                        color: #333333;
                    }}
                    .reset-link {{
                        color: #007bff;
                        text-decoration: none;
                        font-weight: bold;
                        transition: text-decoration 0.3s ease; 
                    }}
                    .reset-link:hover {{
                        text-decoration: underline !important;
                    }}
                    .footer {{
                        margin-top: 10px;
                        text-align: center;
                        font-size: 0.9em;
                        color: #777777;
                    }}
                </style>
            </head>
            <body>
                <div class='email-container'>
                    <div class='email-header'>
                        <h3>Password Reset Request</h3>
                    </div>
                    <div class='email-body'>
                        <p>Dear User,</p>
                        <p>We received a request to reset your password. Click the link below to reset your password:</p>
                        <p><a href='{resetPasswordLink}' class='reset-link'>Reset Your Password</a></p>
                        <p>If you did not request a password reset, please ignore this email.</p>
                        <p>Thank you,<br/>AT InfoTech</p>
                    </div>
                    <div class='footer'>
                        <p>&copy; 2024 AT InfoTech. All rights reserved.</p>
                    </div>
                </div>
            </body>
            </html>";

            message.Body = builder.ToMessageBody();

            using var smtp = new SmtpClient();
            smtp.Connect(emailSettings.Host, emailSettings.Port, SecureSocketOptions.StartTls);
            smtp.Authenticate(emailSettings.Email, emailSettings.Password);
            smtp.Send(message);
            smtp.Disconnect(true);
        }
        public void SendPasswordResetConfirmation(string toEmail)
        {
            var email = new MimeMessage();
            email.Sender = MailboxAddress.Parse(emailSettings.Email);
            email.To.Add(MailboxAddress.Parse(toEmail));
            email.Subject = "Password Reset Successful";
            var builder = new BodyBuilder();
            // Include the login link in the email body
            builder.HtmlBody = @"
                <html>
                <head>
                    <style>
                        body {
                            font-family: Arial, sans-serif;
                            background-color: #f6f6f6;
                            margin: 0;
                            padding: 0;
                        }
                        .email-container {
                            background-color: #ffffff;
                            margin: 50px auto;
                            padding: 20px;
                            max-width: 600px;
                            border-radius: 10px;
                            box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
                        }
                        .header {
                            font-size: 24px;
                            font-weight: bold;
                            text-align: center;
                            color: #333333;
                            margin-bottom: 20px;
                        }
                        .body-content {
                            font-size: 16px;
                            color: #555555;
                            line-height: 1.6;
                        }
                        .footer {
                            text-align: center;
                            font-size: 14px;
                            color: #999999;
                            margin-top: 20px;
                        }
                    </style>
                </head>
                <body>
                    <div class='email-container'>
                        <div class='header'>Password Reset Successful</div>
                        <div class='body-content'>
                            <p>Dear User,</p>
                            <p>Your password has been successfully reset. You can now log in using your new password.</p>
                            <p>If you did not request this change, please contact our support team immediately.</p>
                        </div>
                        <div class='footer'>
                            <p>Thank you for using our service.</p>
                            <p>If you have any questions, feel free to <a href='http://localhost:3000/contact'>contact us</a>.</p>
                        </div>
                    </div>
                </body>
                </html>";

            email.Body = builder.ToMessageBody();

            using var smtp = new SmtpClient();
            smtp.Connect(emailSettings.Host, emailSettings.Port, SecureSocketOptions.StartTls);
            smtp.Authenticate(emailSettings.Email, emailSettings.Password);
            smtp.Send(email);
            smtp.Disconnect(true);
        }
        private string GetOtpEmailBody(string otp)
        {
            return $@"
                <html>
                <body style='font-family: Arial, sans-serif; color: #333333;'>
                    <div style='max-width: 600px; margin: auto; padding: 20px; border: 1px solid #dddddd; border-radius: 10px;'>
                        <h2 style='color: #007bff; text-align: center;'>OTP for Login</h2>
                        <p style='font-size: 16px;'>Dear User,</p>
                        <p style='font-size: 16px;'>Your OTP for Login is:</p>
                        <p style='font-size: 24px; font-weight: bold; text-align: center;'>{otp}</p>
                        <p style='font-size: 16px;'>Please use this OTP within the next 10 minutes.</p>
                        <p style='font-size: 16px;'>Thank you!</p>
                        <p style='font-size: 16px;'>Best regards,<br>Karthick InfoTech</p>
                    </div>
                </body>
                </html>";
        }
        private string GetAccountCreatedEmailBody(string resetLink)
        {
            return $@"
                <html>
                <body style='font-family: Arial, sans-serif; color: #333333;'>
                    <div style='max-width: 600px; margin: auto; padding: 20px; border: 1px solid #dddddd; border-radius: 10px;'>
                        <h2 style='color: #007bff; text-align: center;'>Welcome to Our Service</h2>
                        <p style='font-size: 16px;'>Dear User,</p>
                        <p style='font-size: 16px;'>Your user account has been created successfully.</p>
                        <p style='font-size: 16px;'>To set your password, please click the link below:</p>
                          <p style='text-align: center;'><a href='{resetLink}' style='color: #007bff; text-decoration: underline;'>Click here to Reset Password</a></p>
                        <p style='font-size: 16px;'>If you did not request this, please ignore this email.</p>
                        <p style='font-size: 16px;'>Thank you!</p>
                        <p style='font-size: 16px;'>Best regards,<br>Karthick InfoTech</p>
                    </div>
                </body>
                </html>";
        }
      
    }
}
