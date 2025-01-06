using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace MiliNeu.Models
{
    public class Address
    {
        public int Id { get; set; }
        [JsonIgnore]
        public ApplicationUser User { get; set; }
        [Required]
        public string UserId { get; set; }

        [Required]
        public string FirstName { get; set; } // Name of the person or entity
        public string LastName { get; set; } // Name of the person or entity


        [Required]
        public string StreetAddress { get; set; } // Street address, e.g., "123 Main St"

        public string ApartmentSuite { get; set; } // Optional: Apartment number or Suite number

        [Required]
        public string City { get; set; } // City

        [Required]
        public string State { get; set; } // State or province

        [Required]
        public string PostalCode { get; set; } // Postal or ZIP code

        [Required]
        public string Country { get; set; } // Country

        public string PhoneNumber { get; set; } // Optional: Phone number for shipping

        public string Email { get; set; } // Optional: Email address for shipping updates

        public bool IsPrimary { get; set; } // Is this the default address for billing or shipping?

    }

}
