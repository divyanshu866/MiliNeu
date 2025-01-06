using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace MiliNeu.Models.ViewModels
{
    public class CollctionEditViewModel : CollectionCreateViewModel
    {
        [Key]
        public int Id { get; set; }

        // Existing images to display in the edit view
        public List<CollectionImage> ExistingImages { get; set; }




        public CollctionEditViewModel()
        {
            UploadedImages = new List<IFormFile>();
            ExistingImages = new List<CollectionImage>();

        }
    }
}
