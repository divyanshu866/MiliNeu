using Microsoft.AspNetCore.Http;

namespace MiliNeu.Models.ViewModels
{
    public class HeroSectionVM
    {
        public int? Id { get; set; }
        public string Title { get; set; } // Optional title or tagline
        public string Link { get; set; } // Link to a collection or promotion
        public bool IsActive { get; set; } // Toggle active/inactive hero images
        public IFormFile? UploadedImage { get; set; }
    }
}
