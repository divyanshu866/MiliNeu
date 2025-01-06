using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MiliNeu.Models
{
    public class ProductVariant
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int ColorId { get; set; }

        [ForeignKey("ColorId")]
        public Color Color { get; set; }
        // Foreign Key for Product
        public int ProductId { get; set; } // Foreign key property
        [ForeignKey("ProductId")]
        public Product Product { get; set; }



        // Image collection for each color option
        public List<VariantImage> Images { get; set; } = new List<VariantImage>(); // Ensure initialization
        public bool isMain { get; set; }
        public bool IsDiscontinued { get; set; }
    }
}
