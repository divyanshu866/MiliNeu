using Microsoft.AspNetCore.Http;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MiliNeu.Models.ViewModels
{
    public class CollectionCreateViewModel
    {
        [Required]
        [DisplayName("Collection Name")]
        public string Name { get; set; }

        [Required]
        public string Category { get; set; }

        [Required]
        public string Description { get; set; }

        public decimal Price { get; set; }



        public List<IFormFile> UploadedImages { get; set; }
    }
}
