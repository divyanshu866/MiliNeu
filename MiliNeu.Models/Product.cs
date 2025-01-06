using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MiliNeu.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Category { get; set; }


        [Required]
        public string Description { get; set; }
        [Required]
        public string SizeChartPath { get; set; }
        [Required]
        public DateTime ReleaseDate { get; set; } = DateTime.Now;  // New property to track release date

        public decimal Price { get; set; }
        public decimal? DiscountedPrice { get; set; }
        //Nav Prop right
        public List<ProductVariant> Variants { get; set; } = new List<ProductVariant>(); // One-to-many relationship

        [ValidateNever]
        //Nav Prop Left
        [ForeignKey("CollectionId")]
        public Collection Collection { get; set; }
        public int CollectionId { get; set; }
        public int SalesCount { get; set; }

        public bool IsDiscontinued { get; set; } // Indicates the product is no longer for sale

    }
}
