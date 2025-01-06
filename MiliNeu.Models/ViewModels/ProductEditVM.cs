using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;


namespace MiliNeu.Models.ViewModels
{
    public class ProductEditVM
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Category { get; set; }

        [Required]
        public string Description { get; set; }
        [Required]
        public int CollectionId { get; set; }
        [Required]
        public decimal Price { get; set; }
        public decimal? DiscountedPrice { get; set; }

        public IFormFile? SizeChartImage { get; set; }

        public List<IFormFile>? UploadedImages { get; set; } = new List<IFormFile>();


        [Required]
        public bool IsDiscontinued { get; set; }
        public int productId { get; set; }

        // Existing images to display in the edit view
        public ProductVariantEditVM SelectedProductVariant { get; set; }
        public int OriginalProductVariantId { get; set; }
        public int OriginalColorId { get; set; }
        public string mainImageName { get; set; }


    }



}

