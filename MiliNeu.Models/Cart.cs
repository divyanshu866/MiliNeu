using System.ComponentModel.DataAnnotations;

namespace MiliNeu.Models
{
    public class Cart
    {
        [Key]
        public int Id { get; set; }

        //Nav Prop Left
        [Required]
        public ApplicationUser User { get; set; }

        // Nav Prop Right
        public List<CartItem>? Items { get; set; }

        public Cart()
        {

        }

    }
}
