using System.ComponentModel.DataAnnotations.Schema;

namespace MiliNeu.Models
{
    public class VariantImage : Image
    {
        // Foreign Key to ProductVariant
        [ForeignKey("ProductVariantId")]
        public int ProductVariantId { get; set; }
        public ProductVariant ProductVariant { get; set; }
        public bool IsMain { get; set; } // Indicates if this image is the main one

    }
}
