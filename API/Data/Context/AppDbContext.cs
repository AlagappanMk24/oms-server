using API.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Data.Context
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<InvoiceItem> InvoiceItems { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Currency> Currencies { get; set; }
        public DbSet<Timezone> Timezones { get; set; }
        public DbSet<User> Users { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Seed Currency Data
            modelBuilder.Entity<Currency>().HasData(
                new Currency { Id = 1, Code = "USD", Symbol = "$", Name = "US Dollar" },
                new Currency { Id = 2, Code = "EUR", Symbol = "€", Name = "Euro" },
                new Currency { Id = 3, Code = "JPY", Symbol = "¥", Name = "Japanese Yen" },
                new Currency { Id = 4, Code = "GBP", Symbol = "£", Name = "British Pound" },
                new Currency { Id = 5, Code = "AUD", Symbol = "A$", Name = "Australian Dollar" },
                new Currency { Id = 6, Code = "CAD", Symbol = "C$", Name = "Canadian Dollar" },
                new Currency { Id = 7, Code = "CHF", Symbol = "CHF", Name = "Swiss Franc" },
                new Currency { Id = 8, Code = "CNY", Symbol = "¥", Name = "Chinese Yuan" },
                new Currency { Id = 9, Code = "SEK", Symbol = "kr", Name = "Swedish Krona" },
                new Currency { Id = 10, Code = "NZD", Symbol = "NZ$", Name = "New Zealand Dollar" },
                new Currency { Id = 11, Code = "INR", Symbol = "₹", Name = "Indian Rupee" }
            );

            // Seed Timezone Data
            modelBuilder.Entity<Timezone>().HasData(
                new Timezone { Id = 1, Name = "Pacific Standard Time", UtcOffset = "-08:00", UtcOffsetString = "-08:00", Abbreviation = "PST" },
                new Timezone { Id = 2, Name = "Eastern Standard Time", UtcOffset = "-05:00", UtcOffsetString = "-05:00", Abbreviation = "EST" },
                new Timezone { Id = 3, Name = "Central European Time", UtcOffset = "+01:00", UtcOffsetString = "+01:00", Abbreviation = "CET" },
                new Timezone { Id = 4, Name = "Greenwich Mean Time", UtcOffset = "+00:00", UtcOffsetString = "+00:00", Abbreviation = "GMT" },
                new Timezone { Id = 5, Name = "Australian Eastern Standard Time", UtcOffset = "+10:00", UtcOffsetString = "+10:00", Abbreviation = "AEST" },
                new Timezone { Id = 6, Name = "India Standard Time", UtcOffset = "+05:30", UtcOffsetString = "+05:30", Abbreviation = "IST" },
                new Timezone { Id = 7, Name = "Japan Standard Time", UtcOffset = "+09:00", UtcOffsetString = "+09:00", Abbreviation = "JST" },
                new Timezone { Id = 8, Name = "Brazilia Time", UtcOffset = "-03:00", UtcOffsetString = "-03:00", Abbreviation = "BRT" },
                new Timezone { Id = 9, Name = "Mountain Standard Time", UtcOffset = "-07:00", UtcOffsetString = "-07:00", Abbreviation = "MST" },
                new Timezone { Id = 10, Name = "Singapore Time", UtcOffset = "+08:00", UtcOffsetString = "+08:00", Abbreviation = "SGT" }
            );

            // Seed Location Data
            modelBuilder.Entity<Location>().HasData(
                    new Location
                    {
                        Id = 1,
                        CompanyName = "KLT InfoTech",
                        Email = "admin@kltinfotech_us.com",
                        Phone = "+1-212-555-1234",
                        Address = "123 Business St",
                        City = "New York",
                        State = "NY",
                        Country = "USA",
                        PostalCode = "10001",
                        CurrencyId = 1, // USD
                        TimezoneId = 1, // PST
                        Logo = "logo_us.png",
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now
                    },
                    new Location
                    {
                        Id = 2,
                        CompanyName = "KLT InfoTech",
                        Email = "admin@kltinfotech_gm.com",
                        Phone = "+49-30-555-1234",
                        Address = "456 Market Rd",
                        City = "Berlin",
                        State = "Berlin",
                        Country = "Germany",
                        PostalCode = "10115",
                        CurrencyId = 2, // EUR
                        TimezoneId = 3, // CET
                        Logo = "logo_de.png",
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now
                    },
                    new Location
                    {
                        Id = 3,
                        CompanyName = "KLT InfoTech",
                        Email = "admin@kltinfotech_jn.com",
                        Phone = "+81-3-555-1234",
                        Address = "789 Sakura Ave",
                        City = "Tokyo",
                        State = "Tokyo",
                        Country = "Japan",
                        PostalCode = "100-0001",
                        CurrencyId = 3, // JPY
                        TimezoneId = 7, // JST
                        Logo = "logo_jp.png",
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now
                    },
                    new Location
                    {
                        Id = 4,
                        CompanyName = "KLT InfoTech",
                        Email = "admin@kltinfotech_uk.com",
                        Phone = "+44-20-555-1234",
                        Address = "321 King St",
                        City = "London",
                        State = "London",
                        Country = "UK",
                        PostalCode = "SW1A 1AA",
                        CurrencyId = 4, // GBP
                        TimezoneId = 4, // GMT
                        Logo = "logo_gb.png",
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now
                    },
                    new Location
                    {
                        Id = 5,
                        CompanyName = "KLT InfoTech",
                        Email = "admin@kltinfotech_as.com",
                        Phone = "+61-2-555-1234",
                        Address = "654 Queen St",
                        City = "Sydney",
                        State = "NSW",
                        Country = "Australia",
                        PostalCode = "2000",
                        CurrencyId = 5, // AUD
                        TimezoneId = 5, // AEST
                        Logo = "logo_au.png",
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now
                    },
                    new Location
                    {
                        Id = 6,
                        CompanyName = "KLT InfoTech",
                        Email = "admin@kltinfotech_cn.com",
                        Phone = "+1-416-555-1234",
                        Address = "123 Maple St",
                        City = "Toronto",
                        State = "ON",
                        Country = "Canada",
                        PostalCode = "M5A 1A1",
                        CurrencyId = 6, // CAD
                        TimezoneId = 9, // MST
                        Logo = "logo_ca.png",
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now
                    },
                    new Location
                    {
                        Id = 7,
                        CompanyName = "KLT InfoTech",
                        Email = "admin@kltinfotech_sw.com",
                        Phone = "+41-44-555-1234",
                        Address = "456 Elm St",
                        City = "Zurich",
                        State = "Zurich",
                        Country = "Switzerland",
                        PostalCode = "8001",
                        CurrencyId = 7, // CHF
                        TimezoneId = 4, // GMT
                        Logo = "logo_ch.png",
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now
                    },
                    new Location
                    {
                        Id = 8,
                        CompanyName = "KLT InfoTech",
                        Email = "admin@kltinfotech_ch.com",
                        Phone = "+86-10-555-1234",
                        Address = "789 Bamboo Rd",
                        City = "Beijing",
                        State = "Beijing",
                        Country = "China",
                        PostalCode = "100000",
                        CurrencyId = 8, // CNY
                        TimezoneId = 10, // SGT
                        Logo = "logo_cn.png",
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now
                    },
                    new Location
                    {
                        Id = 9,
                        CompanyName = "KLT InfoTech",
                        Email = "admin@kltinfotech_sw.com",
                        Phone = "+46-8-555-1234",
                        Address = "321 Fjall Rd",
                        City = "Stockholm",
                        State = "Stockholm",
                        Country = "Sweden",
                        PostalCode = "111 22",
                        CurrencyId = 9, // SEK
                        TimezoneId = 3, // CET
                        Logo = "logo_se.png",
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now
                    },
                    new Location
                    {
                        Id = 10,
                        CompanyName = "KLT InfoTech",
                        Email = "admin@kltinfotech_nz.com",
                        Phone = "+64-9-555-1234",
                        Address = "654 Kiwi St",
                        City = "Auckland",
                        State = "Auckland",
                        Country = "New Zealand",
                        PostalCode = "1010",
                        CurrencyId = 10, // NZD
                        TimezoneId = 5, // AEST
                        Logo = "logo_nz.png",
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now
                    },
                    new Location
                    {
                        Id = 11,
                        CompanyName = "KLT InfoTech",
                        Email = "admin@kltinfotech_in.com",
                        Phone = "+91-22-555-1234",
                        Address = "123 Curry Rd",
                        City = "Mumbai",
                        State = "MH",
                        Country = "India",
                        PostalCode = "400001",
                        CurrencyId = 11, // INR
                        TimezoneId = 6, // IST
                        Logo = "logo_in.png",
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now
                    }
            );

            base.OnModelCreating(modelBuilder);

            // Configure the one-to-many relationship between Customer and Order
            modelBuilder.Entity<Customer>()
                .HasMany(c => c.Orders)
                .WithOne(o => o.Customer)
                .HasForeignKey(o => o.CustomerId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configure Customer Address as Owned Entity
            modelBuilder.Entity<Customer>()
                .OwnsOne(c => c.Address, a =>
                {
                    a.Property(p => p.Address1).HasColumnName("Address1");
                    a.Property(p => p.Address2).HasColumnName("Address2");
                    a.Property(p => p.City).HasColumnName("City");
                    a.Property(p => p.State).HasColumnName("State");
                    a.Property(p => p.Country).HasColumnName("Country");
                    a.Property(p => p.ZipCode).HasColumnName("ZipCode");
                });

            // Configure the one-to-many relationship between Order and OrderItem
            modelBuilder.Entity<Order>()
                .HasMany(o => o.OrderItems)
                .WithOne(oi => oi.Order)
                .HasForeignKey(oi => oi.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configure complex types (owned entities)
            modelBuilder.Entity<Order>()
                .OwnsOne(o => o.ShippingAddress);

            modelBuilder.Entity<Order>()
                .OwnsOne(o => o.BillingAddress);

            // Configure the one-to-many relationship between Product and OrderItem
            modelBuilder.Entity<Product>()
                .HasMany(p => p.OrderItems)
                .WithOne(oi => oi.Product)
                .HasForeignKey(oi => oi.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configure the one-to-many relationship between Customer and Invoice
            modelBuilder.Entity<Customer>()
                .HasMany(c => c.Invoices)
                .WithOne(i => i.Customer)
                .HasForeignKey(i => i.CustomerId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configure the one-to-one relationship between Order and Invoice
            modelBuilder.Entity<Order>()
                .HasOne(o => o.Invoice)
                .WithOne(i => i.Order)
                .HasForeignKey<Order>(o => o.InvoiceId)
                .OnDelete(DeleteBehavior.Restrict); // or .OnDelete(DeleteBehavior.Cascade) depending on your needs

            // Configure the one-to-many relationship between Invoice and InvoiceItem
            modelBuilder.Entity<Invoice>()
                .HasMany(i => i.InvoiceItems)
                .WithOne(ii => ii.Invoice)
                .HasForeignKey(ii => ii.InvoiceId);

            // Configure the one-to-many relationship between Invoice and InvoiceAttachments
            modelBuilder.Entity<Invoice>()
                .HasMany(i => i.InvoiceAttachments)
                .WithOne(ii => ii.Invoice)
                .HasForeignKey(ii => ii.InvoiceId);
        }
    }
}


// Explanation :

// Customer can have multiple Orders and Invoices.

//Order can have multiple OrderItems and may be associated with one Invoice.

//Product can be included in multiple OrderItems.

//Invoice can have multiple InvoiceItems and may be associated with one Order.

//OrderItem links Order and Product.
//InvoiceItem links Invoice.