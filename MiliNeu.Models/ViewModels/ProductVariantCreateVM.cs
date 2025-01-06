namespace MiliNeu.Models.ViewModels
{
    public class ProductVariantCreateVM : ColorViewModel
    {
        public int VariantId { get; set; }

        public bool VariantDiscontinued { get; set; }

    }
}
