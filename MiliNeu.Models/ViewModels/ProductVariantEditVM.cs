namespace MiliNeu.Models.ViewModels
{
    public class ProductVariantEditVM : ColorViewModel
    {
        public int VariantId { get; set; }
        public List<VariantImage>? ProductImages { get; set; }
        public bool VariantDiscontinued { get; set; }

        public bool isMain { get; set; }

    }
}
