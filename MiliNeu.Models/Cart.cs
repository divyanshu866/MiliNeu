using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MiliNeu.Models
{
    public class Cart
    {
        [Key]
        public int Id { get; set; }

        //Nav Prop Left
        [ForeignKey("ApplicationUserId")]
        public ApplicationUser? User { get; set; }
        public string? ApplicationUserId { get; set; }

        // Nav Prop Right
        public List<CartItem>? Items { get; set; }

        public Cart()
        {

        }

    }
}
