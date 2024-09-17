using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities
{
    public class Location
    {
        [Key]
        public int Id { get; set; }
        public string CompanyName { get; set; }
        public string Email {  get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string PostalCode { get; set; }

        [ForeignKey("Currency")]
        public int CurrencyId { get; set; }
        public Currency Currency { get; set; } // Navigation property


        [ForeignKey("Timezone")]
        public int TimezoneId { get; set; }
        public Timezone Timezone { get; set; } // Navigation property

        public string Logo { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
    public class Currency
    {
        [Key]
        public int Id { get; set; }
        public string Code { get; set; }
        public string Symbol { get; set; }
        public string Name { get; set; }
    }
    public class Timezone
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string UtcOffset { get; set; }
        public string UtcOffsetString { get; set; }
        public string Abbreviation { get; set; }
    }
}
