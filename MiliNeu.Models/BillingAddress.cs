namespace MiliNeu.Models
{
    public class BillingAddress
    {
        public int Id { get; set; }
        public string Line1 { get; set; }
        public string Line2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
        public string VATNumber { get; set; } // Specific to billing
    }
}
