using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Migrations
{
    /// <inheritdoc />
    public partial class AddedNewFieldsInInvoicesAndLocationsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CompanyName",
                table: "Locations",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Locations",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Phone",
                table: "Locations",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "InvoiceStatus",
                table: "Invoices",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Invoices",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Locations",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CompanyName", "CreatedAt", "Email", "Phone", "UpdatedAt" },
                values: new object[] { "KLT InfoTech", new DateTime(2024, 9, 6, 19, 39, 26, 947, DateTimeKind.Local).AddTicks(2855), "admin@kltinfotech_us.com", "+1-212-555-1234", new DateTime(2024, 9, 6, 19, 39, 26, 947, DateTimeKind.Local).AddTicks(2865) });

            migrationBuilder.UpdateData(
                table: "Locations",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CompanyName", "CreatedAt", "Email", "Phone", "UpdatedAt" },
                values: new object[] { "KLT InfoTech", new DateTime(2024, 9, 6, 19, 39, 26, 947, DateTimeKind.Local).AddTicks(2869), "admin@kltinfotech_gm.com", "+49-30-555-1234", new DateTime(2024, 9, 6, 19, 39, 26, 947, DateTimeKind.Local).AddTicks(2869) });

            migrationBuilder.UpdateData(
                table: "Locations",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CompanyName", "CreatedAt", "Email", "Phone", "UpdatedAt" },
                values: new object[] { "KLT InfoTech", new DateTime(2024, 9, 6, 19, 39, 26, 947, DateTimeKind.Local).AddTicks(2872), "admin@kltinfotech_jn.com", "+81-3-555-1234", new DateTime(2024, 9, 6, 19, 39, 26, 947, DateTimeKind.Local).AddTicks(2872) });

            migrationBuilder.UpdateData(
                table: "Locations",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CompanyName", "CreatedAt", "Email", "Phone", "UpdatedAt" },
                values: new object[] { "KLT InfoTech", new DateTime(2024, 9, 6, 19, 39, 26, 947, DateTimeKind.Local).AddTicks(2875), "admin@kltinfotech_uk.com", "+44-20-555-1234", new DateTime(2024, 9, 6, 19, 39, 26, 947, DateTimeKind.Local).AddTicks(2875) });

            migrationBuilder.UpdateData(
                table: "Locations",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CompanyName", "CreatedAt", "Email", "Phone", "UpdatedAt" },
                values: new object[] { "KLT InfoTech", new DateTime(2024, 9, 6, 19, 39, 26, 947, DateTimeKind.Local).AddTicks(2878), "admin@kltinfotech_as.com", "+61-2-555-1234", new DateTime(2024, 9, 6, 19, 39, 26, 947, DateTimeKind.Local).AddTicks(2878) });

            migrationBuilder.UpdateData(
                table: "Locations",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CompanyName", "CreatedAt", "Email", "Phone", "UpdatedAt" },
                values: new object[] { "KLT InfoTech", new DateTime(2024, 9, 6, 19, 39, 26, 947, DateTimeKind.Local).AddTicks(2880), "admin@kltinfotech_cn.com", "+1-416-555-1234", new DateTime(2024, 9, 6, 19, 39, 26, 947, DateTimeKind.Local).AddTicks(2881) });

            migrationBuilder.UpdateData(
                table: "Locations",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CompanyName", "CreatedAt", "Email", "Phone", "UpdatedAt" },
                values: new object[] { "KLT InfoTech", new DateTime(2024, 9, 6, 19, 39, 26, 947, DateTimeKind.Local).AddTicks(2883), "admin@kltinfotech_sw.com", "+41-44-555-1234", new DateTime(2024, 9, 6, 19, 39, 26, 947, DateTimeKind.Local).AddTicks(2884) });

            migrationBuilder.UpdateData(
                table: "Locations",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CompanyName", "CreatedAt", "Email", "Phone", "UpdatedAt" },
                values: new object[] { "KLT InfoTech", new DateTime(2024, 9, 6, 19, 39, 26, 947, DateTimeKind.Local).AddTicks(2887), "admin@kltinfotech_ch.com", "+86-10-555-1234", new DateTime(2024, 9, 6, 19, 39, 26, 947, DateTimeKind.Local).AddTicks(2887) });

            migrationBuilder.UpdateData(
                table: "Locations",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "CompanyName", "CreatedAt", "Email", "Phone", "UpdatedAt" },
                values: new object[] { "KLT InfoTech", new DateTime(2024, 9, 6, 19, 39, 26, 947, DateTimeKind.Local).AddTicks(2889), "admin@kltinfotech_sw.com", "+46-8-555-1234", new DateTime(2024, 9, 6, 19, 39, 26, 947, DateTimeKind.Local).AddTicks(2890) });

            migrationBuilder.UpdateData(
                table: "Locations",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "CompanyName", "CreatedAt", "Email", "Phone", "UpdatedAt" },
                values: new object[] { "KLT InfoTech", new DateTime(2024, 9, 6, 19, 39, 26, 947, DateTimeKind.Local).AddTicks(2892), "admin@kltinfotech_nz.com", "+64-9-555-1234", new DateTime(2024, 9, 6, 19, 39, 26, 947, DateTimeKind.Local).AddTicks(2893) });

            migrationBuilder.UpdateData(
                table: "Locations",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "CompanyName", "CreatedAt", "Email", "Phone", "UpdatedAt" },
                values: new object[] { "KLT InfoTech", new DateTime(2024, 9, 6, 19, 39, 26, 947, DateTimeKind.Local).AddTicks(2895), "admin@kltinfotech_in.com", "+91-22-555-1234", new DateTime(2024, 9, 6, 19, 39, 26, 947, DateTimeKind.Local).AddTicks(2896) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CompanyName",
                table: "Locations");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Locations");

            migrationBuilder.DropColumn(
                name: "Phone",
                table: "Locations");

            migrationBuilder.DropColumn(
                name: "InvoiceStatus",
                table: "Invoices");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Invoices");

            migrationBuilder.UpdateData(
                table: "Locations",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 8, 9, 18, 55, 19, 98, DateTimeKind.Local).AddTicks(3470), new DateTime(2024, 8, 9, 18, 55, 19, 98, DateTimeKind.Local).AddTicks(3488) });

            migrationBuilder.UpdateData(
                table: "Locations",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 8, 9, 18, 55, 19, 98, DateTimeKind.Local).AddTicks(3492), new DateTime(2024, 8, 9, 18, 55, 19, 98, DateTimeKind.Local).AddTicks(3493) });

            migrationBuilder.UpdateData(
                table: "Locations",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 8, 9, 18, 55, 19, 98, DateTimeKind.Local).AddTicks(3495), new DateTime(2024, 8, 9, 18, 55, 19, 98, DateTimeKind.Local).AddTicks(3496) });

            migrationBuilder.UpdateData(
                table: "Locations",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 8, 9, 18, 55, 19, 98, DateTimeKind.Local).AddTicks(3499), new DateTime(2024, 8, 9, 18, 55, 19, 98, DateTimeKind.Local).AddTicks(3500) });

            migrationBuilder.UpdateData(
                table: "Locations",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 8, 9, 18, 55, 19, 98, DateTimeKind.Local).AddTicks(3502), new DateTime(2024, 8, 9, 18, 55, 19, 98, DateTimeKind.Local).AddTicks(3503) });

            migrationBuilder.UpdateData(
                table: "Locations",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 8, 9, 18, 55, 19, 98, DateTimeKind.Local).AddTicks(3506), new DateTime(2024, 8, 9, 18, 55, 19, 98, DateTimeKind.Local).AddTicks(3506) });

            migrationBuilder.UpdateData(
                table: "Locations",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 8, 9, 18, 55, 19, 98, DateTimeKind.Local).AddTicks(3509), new DateTime(2024, 8, 9, 18, 55, 19, 98, DateTimeKind.Local).AddTicks(3510) });

            migrationBuilder.UpdateData(
                table: "Locations",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 8, 9, 18, 55, 19, 98, DateTimeKind.Local).AddTicks(3512), new DateTime(2024, 8, 9, 18, 55, 19, 98, DateTimeKind.Local).AddTicks(3513) });

            migrationBuilder.UpdateData(
                table: "Locations",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 8, 9, 18, 55, 19, 98, DateTimeKind.Local).AddTicks(3516), new DateTime(2024, 8, 9, 18, 55, 19, 98, DateTimeKind.Local).AddTicks(3517) });

            migrationBuilder.UpdateData(
                table: "Locations",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 8, 9, 18, 55, 19, 98, DateTimeKind.Local).AddTicks(3523), new DateTime(2024, 8, 9, 18, 55, 19, 98, DateTimeKind.Local).AddTicks(3524) });

            migrationBuilder.UpdateData(
                table: "Locations",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 8, 9, 18, 55, 19, 98, DateTimeKind.Local).AddTicks(3526), new DateTime(2024, 8, 9, 18, 55, 19, 98, DateTimeKind.Local).AddTicks(3527) });
        }
    }
}
