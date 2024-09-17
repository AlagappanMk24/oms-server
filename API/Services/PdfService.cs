using API.DTOs;
using API.Entities;
using API.Services.Interface;
using iText.IO.Font;
using iText.IO.Font.Constants;
using iText.IO.Image;
using iText.Kernel.Colors;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Layout.Borders;
using iText.Layout.Element;
using iText.Layout.Properties;

namespace API.Services
{
    public class PdfService : IPdfService
    {
        public byte[] GenerateInvoicePdf(Invoice invoice)
        {
            using (var stream = new MemoryStream())
            {
                PdfWriter writer = new PdfWriter(stream);
                PdfDocument pdfDoc = new PdfDocument(writer);
                iText.Layout.Document document = new iText.Layout.Document(pdfDoc);

                document.SetBackgroundColor(ColorConstants.WHITE);

                string fontPath = "wwwroot/fonts/arial-unicode-ms.ttf";
                // Load the new font
                PdfFont font = PdfFontFactory.CreateFont(fontPath, PdfEncodings.IDENTITY_H);
                document.SetFont(font);

                // Header Section
                var headerTable = new Table(2).UseAllAvailableWidth();

                if (!string.IsNullOrEmpty(invoice?.Location?.Logo))
                {
                    ImageData imageData = ImageDataFactory.Create(invoice.Location.Logo);
                    Image logoImage = new Image(imageData)
                        .SetWidth(40)
                        .SetHeight(40)
                        .SetAutoScale(true);
                    headerTable.AddCell(new Cell().Add(logoImage).SetBorder(Border.NO_BORDER));
                }
                else
                {
                    headerTable.AddCell(new Cell().Add(new Paragraph()).SetBorder(Border.NO_BORDER));
                }
                string invoiceTotal;

                if (invoice?.Location?.Currency?.Symbol == "₹")
                {
                    invoiceTotal = $"Invoice Total: Rs.{invoice?.TotalAmount:F2}";
                }
                else
                {
                    invoiceTotal = $"Invoice Total: {invoice?.Location?.Currency?.Symbol}{invoice?.TotalAmount:F2}";
                }


                var headerRight = new Cell().Add(new Paragraph("INVOICE")
                    .SetFontSize(24)
                    .SetFontColor(ColorConstants.BLACK))
                    .Add(new Paragraph(invoiceTotal)
                    .SetFontSize(18)
                    .SetFontColor(ColorConstants.RED))
                    .Add(new Paragraph($"Invoice Number: {invoice?.InvoiceNumber}")
                    .SetFontSize(12)
                    .SetFontColor(ColorConstants.GRAY))
                    .Add(new Paragraph($"Date Of Issue: {invoice?.CreatedAt:MM/dd/yyyy}")
                    .SetFontSize(12)
                    .SetFontColor(ColorConstants.GRAY))
                    .SetTextAlignment(TextAlignment.RIGHT)
                    .SetBorder(Border.NO_BORDER);
                headerTable.AddCell(headerRight);

                document.Add(headerTable.SetMarginBottom(20));

                // Billing and Company Info Section
                var billingTable = new Table(2).UseAllAvailableWidth();
                billingTable.SetMarginBottom(20); // Margin Bottom

                // Customer Info Cell
                billingTable.AddCell(new Cell()
                    .Add(new Paragraph($"Billed To: {invoice?.Customer?.Name}")
                        .SetFontSize(14)
                        .SetBold()
                        .SetFontColor(ColorConstants.DARK_GRAY) // Text Color
                        .SetMarginBottom(10)) // Margin Bottom
                    .Add(new Paragraph($"{invoice?.Customer?.Email}")
                        .SetFontSize(12)
                        .SetFontColor(ColorConstants.DARK_GRAY))
                    .Add(new Paragraph($"{invoice?.Customer?.PhoneNumber}")
                        .SetFontSize(12)
                        .SetFontColor(ColorConstants.DARK_GRAY))
                    .Add(new Paragraph($"{invoice?.Customer?.Address?.Address1}")
                        .SetFontSize(12)
                        .SetFontColor(ColorConstants.DARK_GRAY))
                    .Add(new Paragraph($"{invoice?.Customer?.Address?.City}, {invoice?.Customer?.Address?.State}")
                        .SetFontSize(12)
                        .SetFontColor(ColorConstants.DARK_GRAY))
                    .Add(new Paragraph($"{invoice?.Customer?.Address?.Country} - {invoice?.Customer?.Address?.ZipCode}")
                        .SetFontSize(12)
                        .SetFontColor(ColorConstants.DARK_GRAY))
                    .SetBackgroundColor(new DeviceRgb(245, 245, 245)) // Set background color (Gray 50)
                    .SetPadding(10) // Padding
                    .SetBorder(Border.NO_BORDER)); // Remove border

                // Company Info Cell
                billingTable.AddCell(new Cell()
                    .Add(new Paragraph("KLT InfoTech")
                        .SetFontSize(14)
                        .SetBold())
                    .Add(new Paragraph("0123456789")
                        .SetFontSize(12)
                        .SetFontColor(ColorConstants.DARK_GRAY))
                    .Add(new Paragraph("admin@bookamat.co")
                        .SetFontSize(12)
                        .SetFontColor(ColorConstants.DARK_GRAY))
                    .Add(new Paragraph("27 Kooyong Road, Armadale, Victoria")
                        .SetFontSize(12)
                        .SetFontColor(ColorConstants.DARK_GRAY))
                    .SetTextAlignment(TextAlignment.RIGHT)
                    .SetBackgroundColor(new DeviceRgb(245, 245, 245)) // Set background color (Gray 50)
                    .SetPadding(10) // Padding
                    .SetBorder(Border.NO_BORDER));

                document.Add(billingTable);


                // Invoice Items Table
                var itemsTable = new iText.Layout.Element.Table(new float[] { 2, 4, 2, 2, 2 }).UseAllAvailableWidth();
                itemsTable.AddHeaderCell(new Cell().Add(new Paragraph("Service").SetBold()));
                itemsTable.AddHeaderCell(new Cell().Add(new Paragraph("Description").SetBold()));
                itemsTable.AddHeaderCell(new Cell().Add(new Paragraph("Unit").SetBold()));
                itemsTable.AddHeaderCell(new Cell().Add(new Paragraph("Price").SetBold()));
                itemsTable.AddHeaderCell(new Cell().Add(new Paragraph("Amount").SetBold()));

                foreach (var item in invoice.InvoiceItems)
                {
                    itemsTable.AddCell(new Cell().Add(new Paragraph(item.Service)));
                    itemsTable.AddCell(new Cell().Add(new Paragraph(item.Description)));
                    itemsTable.AddCell(new Cell().Add(new Paragraph(item.Unit.ToString()).SetTextAlignment(TextAlignment.RIGHT)));
                    itemsTable.AddCell(new Cell().Add(new Paragraph($"{invoice?.Location?.Currency?.Symbol}{item.Price:F2}").SetTextAlignment(TextAlignment.RIGHT)));
                    itemsTable.AddCell(new Cell().Add(new Paragraph($"{invoice?.Location?.Currency?.Symbol}{item.Amount:F2}").SetTextAlignment(TextAlignment.RIGHT)));
                }

                document.Add(itemsTable);

                // Total Calculation
                var totalTable = new iText.Layout.Element.Table(2).UseAllAvailableWidth();
                totalTable.AddCell(new Cell().Add(new Paragraph("Sub Total :").SetBold()).SetTextAlignment(TextAlignment.RIGHT));
                totalTable.AddCell(new Cell().Add(new Paragraph($"{invoice?.Location?.Currency?.Symbol}{invoice?.Subtotal:F2}")).SetTextAlignment(TextAlignment.RIGHT));
                totalTable.AddCell(new Cell().Add(new Paragraph("Discount :").SetBold()).SetTextAlignment(TextAlignment.RIGHT));
                totalTable.AddCell(new Cell().Add(new Paragraph($"-{invoice?.Location?.Currency?.Symbol}{invoice?.Discount:F2}")).SetTextAlignment(TextAlignment.RIGHT));
                totalTable.AddCell(new Cell().Add(new Paragraph("Tax :").SetBold()).SetTextAlignment(TextAlignment.RIGHT));
                totalTable.AddCell(new Cell().Add(new Paragraph($"{invoice?.Location?.Currency?.Symbol}{invoice?.Tax:F2}")).SetTextAlignment(TextAlignment.RIGHT));
                totalTable.AddCell(new Cell().Add(new Paragraph("Total Amount :").SetBold().SetFontSize(14).SetFontColor(ColorConstants.BLACK)).SetTextAlignment(TextAlignment.RIGHT));
                totalTable.AddCell(new Cell().Add(new Paragraph($"{invoice?.Location?.Currency?.Symbol}{invoice?.TotalAmount:F2}").SetBold().SetFontSize(14).SetFontColor(ColorConstants.BLACK)).SetTextAlignment(TextAlignment.RIGHT));

                document.Add(totalTable.SetMarginBottom(20));

                // Notes Section
                if (!string.IsNullOrEmpty(invoice?.Notes))
                {
                    var notesParagraph = new Paragraph("Notes:")
                        .SetBold()
                        .SetFontSize(14)
                        .SetFontColor(ColorConstants.BLACK)
                        .Add("\n")
                        .Add(invoice.Notes)
                        .SetFontSize(12)
                        .SetFontColor(ColorConstants.DARK_GRAY);

                    document.Add(notesParagraph.SetMarginBottom(20).SetPadding(10));
                }

                document.Close();

                return stream.ToArray();
            }
        }
    }

}

// Payment Instruction
//var paymentInstruction = new Paragraph("HOW TO PAY:")
//    .SetBold()
//    .SetFontSize(14)
//    .SetFontColor(ColorConstants.BLACK)
//    .Add("\nMake payments through Bank Transfer:")
//    .SetFontSize(12)
//    .SetFontColor(ColorConstants.DARK_GRAY)
//    .Add("\nAccount Name: KLT InfoTech")
//    .Add("\nAccount Number: 1234567890")
//    .Add("\nBank Name: XYZ Bank")
//    .Add("\nSWIFT Code: XYZ123")
//    .Add("\nReference: Invoice Number")
//    .Add("\nPayPal Visit: https://payid.com.au/")
//    //.SetFontColor(ColorConstants.BLUE)
//    .Add("\nPlease Send Proof of Payment To: admin@kltinfotech.com")
//    //.SetFontColor(ColorConstants.RED)
//    .SetFontSize(12)
//    .SetBold();

//document.Add(paymentInstruction.SetMarginBottom(20).SetPadding(10));