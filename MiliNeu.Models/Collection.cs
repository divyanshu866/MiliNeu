
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
namespace MiliNeu.Models
{
    public class Collection
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [DisplayName("Collection Name")]
        public string Name { get; set; }

        [Required]
        public string Category { get; set; }

        [Required]
        public string Description { get; set; }

        public decimal Price { get; set; }

        [ValidateNever]
        public List<CollectionImage> Images { get; set; }

        //Nav Prop
        public List<Product>? Products { get; set; }
        [Required]
        public DateTime ReleaseDate { get; set; } = DateTime.Now;  // New property to track release date




    }
}
