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
        public int Price { get; set; }

        [Required]
        public string Path { get; set; }

        // Nav Prop Right
        public List<CartItem> CartItems { get; set; }

        //Nav Prop Left
        public int CollectionId { get; set; }
        public string CollectionName { get; set; }
        public Collection? Collection { get; set; }
        /*public List<Cart>? Cart { get; set; }*/
        public Product()
        {

        }
    }
}
