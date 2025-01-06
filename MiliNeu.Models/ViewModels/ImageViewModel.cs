namespace MiliNeu.Models.ViewModels
{
    public class ImageViewModel
    {
        public string Path { get; set; }

        public Product Product { get; set; }
        public int ProductId { get; set; }
        public bool IsMain { get; set; } // Indicates if this image is the main one
    }
}
