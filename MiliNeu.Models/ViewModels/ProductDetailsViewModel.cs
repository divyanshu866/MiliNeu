using System.ComponentModel.DataAnnotations;

namespace MiliNeu.Models.ViewModels
{
    public class ProductDetailsViewModel
    {
        public Product Product { get; set; }
        public List<ProductVariant> ProductVariants { get; set; }
        public ProductVariant SelectedVariant { get; set; }
        public IEnumerable<Collection> Collection { get; set; }

        [Required(ErrorMessage = "Please select a size.")]
        public string SelectedSize { get; set; }

    }
}
