namespace MiliNeu.Models.ViewModels
{
    public class OrderItemVM
    {
        public ProductVariantVM OrderedVariant { get; set; }
        public string Name { get; set; }

        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string SelectedSize { get; set; }

    }
}
