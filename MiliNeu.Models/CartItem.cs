using System.ComponentModel.DataAnnotations;

namespace MiliNeu.Models
{
    public class CartItem
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int CartId { get; set; }

        [Required]
        public int ProductId { get; set; }

        [Required]
        public string SelectedSize { get; set; }

        [Required]
        public int Quantity { get; set; }

        public decimal Price { get; set; }

        // Nav Prop Left
        [Required]
        public Cart Cart { get; set; }

        // Nav Prop Right
        [Required]
        public Product Product { get; set; }
        public CartItem()
        {

        }
        public void calculatePrice()
        {
            if (!(Product == null) && !(Quantity == null))
            {
                decimal total = 0;

                Price = (decimal)Quantity * (decimal)Product.Price;
            }
        }
    }
}
