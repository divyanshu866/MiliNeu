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
        public decimal Price { get; set; }

        [ValidateNever]
        public string Path { get; set; }

        [ValidateNever]
        //Nav Prop Left
        [ForeignKey("CollectionId")]
        public Collection Collection { get; set; }
        public int CollectionId { get; set; }
        public Product()
        {

        }
    }
}
