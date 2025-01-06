using System.ComponentModel.DataAnnotations;

namespace MiliNeu.Models.ViewModels
{
    public class ProductVariantVM
    {
        public int Id { get; set; }

        [Required]
        public int ColorId { get; set; }

        public Color Color { get; set; }
        // Foreign Key for Product
        public int ProductId { get; set; } // Foreign key property



        // Image collection for each color option
        public List<VariantImage> Images { get; set; } = new List<VariantImage>(); // Ensure initialization
        public bool isMain { get; set; }
        public bool IsDiscontinued { get; set; }
    }
}
