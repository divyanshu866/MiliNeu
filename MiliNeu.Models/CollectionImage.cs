using System.ComponentModel.DataAnnotations.Schema;

namespace MiliNeu.Models
{
    public class CollectionImage : Image
    {
        public Collection Collection { get; set; }
        [ForeignKey("CollectionId")]
        public int CollectionId { get; set; }
        public bool IsMain { get; set; } // Indicates if this image is the main one

    }
}
