using System.ComponentModel.DataAnnotations.Schema;

namespace MiliNeu.Models
{
    public class HeroSection
    {
        public int Id { get; set; }
        [ForeignKey("HeroSectionImageId")]
        public HeroSectionImage Image { get; set; }
        public int HeroSectionImageId { get; set; }
        public string Title { get; set; } // Optional title or tagline
        public string Link { get; set; } // Link to a collection or promotion
        public bool IsActive { get; set; } // Toggle active/inactive hero images
    }

}
