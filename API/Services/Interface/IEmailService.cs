using API.DTOs;

namespace API.Services.Interface
{
    public interface IEmailService
    {
        void SendEmail(MailRequestDto mailrequest);
        void SendPasswordResetConfirmation(string email);
        void SendResetPasswordLink(string token, string email);
        void SendInvoiceToEmail(InvoiceMailDto invoiceMailDto, byte[] pdfContent = null);
    }
}
