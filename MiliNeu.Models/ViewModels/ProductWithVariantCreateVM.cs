namespace MiliNeu.Models.ViewModels
{
    public class ProductWithVariantCreateVM
    {
        public ProductCreateVM ProductCreateVM { get; set; }
        public ProductVariantCreateVM ProductVariantCreateVM { get; set; }
        public string mainImageName { get; set; }

    }
}
