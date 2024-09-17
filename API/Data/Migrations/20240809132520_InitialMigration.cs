using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace API.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Currencies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Symbol = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Currencies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address1 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address2 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    State = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Country = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ZipCode = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<double>(type: "float", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsInStore = table.Column<bool>(type: "bit", nullable: false),
                    SKU = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Category = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StockQuantity = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Weight = table.Column<double>(type: "float", nullable: true),
                    Dimensions = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Manufacturer = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Rating = table.Column<double>(type: "float", nullable: true),
                    IsFeatured = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Timezones",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UtcOffset = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UtcOffsetString = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Abbreviation = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Timezones", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Salt = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateModified = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    ResetToken = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResetTokenExpiry = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsExternalUser = table.Column<bool>(type: "bit", nullable: false),
                    AuthProvider = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "Locations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    State = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Country = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PostalCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CurrencyId = table.Column<int>(type: "int", nullable: false),
                    TimezoneId = table.Column<int>(type: "int", nullable: false),
                    Logo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Locations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Locations_Currencies_CurrencyId",
                        column: x => x.CurrencyId,
                        principalTable: "Currencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Locations_Timezones_TimezoneId",
                        column: x => x.TimezoneId,
                        principalTable: "Timezones",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Invoices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InvoiceNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PONumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InvoiceDue = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PaymentDue = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CustomerId = table.Column<int>(type: "int", nullable: false),
                    LocationId = table.Column<int>(type: "int", nullable: false),
                    OrderId = table.Column<int>(type: "int", nullable: true),
                    Subtotal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Discount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Tax = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Invoices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Invoices_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Invoices_Locations_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Locations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InvoiceAttachments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AttachmentName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AttachmentContent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InvoiceId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvoiceAttachments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InvoiceAttachments_Invoices_InvoiceId",
                        column: x => x.InvoiceId,
                        principalTable: "Invoices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InvoiceItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Service = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Unit = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    InvoiceId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvoiceItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InvoiceItems_Invoices_InvoiceId",
                        column: x => x.InvoiceId,
                        principalTable: "Invoices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OrderDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    OrderStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ShippingAddress_Address1 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ShippingAddress_Address2 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ShippingAddress_City = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ShippingAddress_State = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ShippingAddress_Country = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ShippingAddress_ZipCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BillingAddress_Address1 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BillingAddress_Address2 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BillingAddress_City = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BillingAddress_State = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BillingAddress_Country = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BillingAddress_ZipCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PaymentMethod = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PaymentStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ShippingCost = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TaxAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Discount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CustomerId = table.Column<int>(type: "int", nullable: false),
                    InvoiceId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Orders_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Orders_Invoices_InvoiceId",
                        column: x => x.InvoiceId,
                        principalTable: "Invoices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "OrderItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderItems_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderItems_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Currencies",
                columns: new[] { "Id", "Code", "Name", "Symbol" },
                values: new object[,]
                {
                    { 1, "USD", "US Dollar", "$" },
                    { 2, "EUR", "Euro", "€" },
                    { 3, "JPY", "Japanese Yen", "¥" },
                    { 4, "GBP", "British Pound", "£" },
                    { 5, "AUD", "Australian Dollar", "A$" },
                    { 6, "CAD", "Canadian Dollar", "C$" },
                    { 7, "CHF", "Swiss Franc", "CHF" },
                    { 8, "CNY", "Chinese Yuan", "¥" },
                    { 9, "SEK", "Swedish Krona", "kr" },
                    { 10, "NZD", "New Zealand Dollar", "NZ$" },
                    { 11, "INR", "Indian Rupee", "₹" }
                });

            migrationBuilder.InsertData(
                table: "Timezones",
                columns: new[] { "Id", "Abbreviation", "Name", "UtcOffset", "UtcOffsetString" },
                values: new object[,]
                {
                    { 1, "PST", "Pacific Standard Time", "-08:00", "-08:00" },
                    { 2, "EST", "Eastern Standard Time", "-05:00", "-05:00" },
                    { 3, "CET", "Central European Time", "+01:00", "+01:00" },
                    { 4, "GMT", "Greenwich Mean Time", "+00:00", "+00:00" },
                    { 5, "AEST", "Australian Eastern Standard Time", "+10:00", "+10:00" },
                    { 6, "IST", "India Standard Time", "+05:30", "+05:30" },
                    { 7, "JST", "Japan Standard Time", "+09:00", "+09:00" },
                    { 8, "BRT", "Brazilia Time", "-03:00", "-03:00" },
                    { 9, "MST", "Mountain Standard Time", "-07:00", "-07:00" },
                    { 10, "SGT", "Singapore Time", "+08:00", "+08:00" }
                });

            migrationBuilder.InsertData(
                table: "Locations",
                columns: new[] { "Id", "Address", "City", "Country", "CreatedAt", "CurrencyId", "Logo", "PostalCode", "State", "TimezoneId", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, "123 Business St", "New York", "USA", new DateTime(2024, 8, 9, 18, 55, 19, 98, DateTimeKind.Local).AddTicks(3470), 1, "logo_us.png", "10001", "NY", 1, new DateTime(2024, 8, 9, 18, 55, 19, 98, DateTimeKind.Local).AddTicks(3488) },
                    { 2, "456 Market Rd", "Berlin", "Germany", new DateTime(2024, 8, 9, 18, 55, 19, 98, DateTimeKind.Local).AddTicks(3492), 2, "logo_de.png", "10115", "Berlin", 3, new DateTime(2024, 8, 9, 18, 55, 19, 98, DateTimeKind.Local).AddTicks(3493) },
                    { 3, "789 Sakura Ave", "Tokyo", "Japan", new DateTime(2024, 8, 9, 18, 55, 19, 98, DateTimeKind.Local).AddTicks(3495), 3, "logo_jp.png", "100-0001", "Tokyo", 7, new DateTime(2024, 8, 9, 18, 55, 19, 98, DateTimeKind.Local).AddTicks(3496) },
                    { 4, "321 King St", "London", "UK", new DateTime(2024, 8, 9, 18, 55, 19, 98, DateTimeKind.Local).AddTicks(3499), 4, "logo_gb.png", "SW1A 1AA", "London", 4, new DateTime(2024, 8, 9, 18, 55, 19, 98, DateTimeKind.Local).AddTicks(3500) },
                    { 5, "654 Queen St", "Sydney", "Australia", new DateTime(2024, 8, 9, 18, 55, 19, 98, DateTimeKind.Local).AddTicks(3502), 5, "logo_au.png", "2000", "NSW", 5, new DateTime(2024, 8, 9, 18, 55, 19, 98, DateTimeKind.Local).AddTicks(3503) },
                    { 6, "123 Maple St", "Toronto", "Canada", new DateTime(2024, 8, 9, 18, 55, 19, 98, DateTimeKind.Local).AddTicks(3506), 6, "logo_ca.png", "M5A 1A1", "ON", 9, new DateTime(2024, 8, 9, 18, 55, 19, 98, DateTimeKind.Local).AddTicks(3506) },
                    { 7, "456 Elm St", "Zurich", "Switzerland", new DateTime(2024, 8, 9, 18, 55, 19, 98, DateTimeKind.Local).AddTicks(3509), 7, "logo_ch.png", "8001", "Zurich", 4, new DateTime(2024, 8, 9, 18, 55, 19, 98, DateTimeKind.Local).AddTicks(3510) },
                    { 8, "789 Bamboo Rd", "Beijing", "China", new DateTime(2024, 8, 9, 18, 55, 19, 98, DateTimeKind.Local).AddTicks(3512), 8, "logo_cn.png", "100000", "Beijing", 10, new DateTime(2024, 8, 9, 18, 55, 19, 98, DateTimeKind.Local).AddTicks(3513) },
                    { 9, "321 Fjall Rd", "Stockholm", "Sweden", new DateTime(2024, 8, 9, 18, 55, 19, 98, DateTimeKind.Local).AddTicks(3516), 9, "logo_se.png", "111 22", "Stockholm", 3, new DateTime(2024, 8, 9, 18, 55, 19, 98, DateTimeKind.Local).AddTicks(3517) },
                    { 10, "654 Kiwi St", "Auckland", "New Zealand", new DateTime(2024, 8, 9, 18, 55, 19, 98, DateTimeKind.Local).AddTicks(3523), 10, "logo_nz.png", "1010", "Auckland", 5, new DateTime(2024, 8, 9, 18, 55, 19, 98, DateTimeKind.Local).AddTicks(3524) },
                    { 11, "123 Curry Rd", "Mumbai", "India", new DateTime(2024, 8, 9, 18, 55, 19, 98, DateTimeKind.Local).AddTicks(3526), 11, "logo_in.png", "400001", "MH", 6, new DateTime(2024, 8, 9, 18, 55, 19, 98, DateTimeKind.Local).AddTicks(3527) }
                });

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceAttachments_InvoiceId",
                table: "InvoiceAttachments",
                column: "InvoiceId");

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceItems_InvoiceId",
                table: "InvoiceItems",
                column: "InvoiceId");

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_CustomerId",
                table: "Invoices",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_LocationId",
                table: "Invoices",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_Locations_CurrencyId",
                table: "Locations",
                column: "CurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_Locations_TimezoneId",
                table: "Locations",
                column: "TimezoneId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_OrderId",
                table: "OrderItems",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_ProductId",
                table: "OrderItems",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_CustomerId",
                table: "Orders",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_InvoiceId",
                table: "Orders",
                column: "InvoiceId",
                unique: true,
                filter: "[InvoiceId] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InvoiceAttachments");

            migrationBuilder.DropTable(
                name: "InvoiceItems");

            migrationBuilder.DropTable(
                name: "OrderItems");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Invoices");

            migrationBuilder.DropTable(
                name: "Customers");

            migrationBuilder.DropTable(
                name: "Locations");

            migrationBuilder.DropTable(
                name: "Currencies");

            migrationBuilder.DropTable(
                name: "Timezones");
        }
    }
}
