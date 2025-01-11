using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace MiliNeu.Models.ViewModels
{
    public class ProductCreateVM
    {

        [Required]
        public string Name { get; set; }

        [Required]
        public int CategoryId { get; set; }

        [Required]
        public string Description { get; set; }
        [Required]
        public int CollectionId { get; set; }
        [Required]
        public IFormFile SizeChartImage { get; set; }
        [Required]
        public decimal Price { get; set; }
        public decimal? DiscountedPrice { get; set; }


        [Required]
        public List<IFormFile> UploadedImages { get; set; } = new List<IFormFile>();


        [Required]
        public bool IsDiscontinued { get; set; }
    }
}
