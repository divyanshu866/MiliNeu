using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MiliNeu.Models
{
    public class OrderAddress
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string FirstName { get; set; } // Full name of the recipient
        public string LastName { get; set; } // Full name of the recipient

        [Required]
        [StringLength(200)]
        public string StreetAddress { get; set; } // Street address, e.g., "123 Main St"

        [StringLength(50)]
        public string ApartmentSuite { get; set; } // Optional: Apartment or Suite number

        [Required]
        [StringLength(100)]
        public string City { get; set; } // City

        [Required]
        [StringLength(100)]
        public string State { get; set; } // State or province

        [Required]
        [StringLength(20)]
        public string PostalCode { get; set; } // Postal or ZIP code

        [Required]
        [StringLength(100)]
        public string Country { get; set; } // Country

        [StringLength(15)]
        public string PhoneNumber { get; set; } // Optional: Phone number for contact

        [StringLength(100)]
        [ForeignKey("OrderId")]
        public string Email { get; set; } // Optional: Email address for updates

        /*        //Nav Prop
                public Order Order { get; set; }
                public int OrderId { get; set; }*/
    }
}
