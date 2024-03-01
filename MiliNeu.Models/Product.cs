using System.ComponentModel.DataAnnotations;

namespace MiliNeu.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Category { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        public string Path { get; set; }

        //Nav Prop Left
        [Required]
        public Collection Collection { get; set; }
        [Required]
        public int CollectionId { get; set; }
        public Product()
        {

        }
    }
}
