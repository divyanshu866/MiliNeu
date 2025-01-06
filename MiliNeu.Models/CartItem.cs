using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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


        // Nav Prop Left
        [Required]
        [ForeignKey("CartId")]
        public Cart Cart { get; set; }

        // Nav Prop Right
        [Required]
        [ForeignKey("ProductId")]
        public Product Product { get; set; }

        public int ProductVariantId { get; set; }  // Foreign Key to ProductVariant
        [Required]
        [ForeignKey("ProductVariantId")]
        public ProductVariant ProductVariant { get; set; }  // Navigation property to ProductVariant






        public CartItem()
        {

        }
        public void calculatePrice()
        {
            /* if (!(Product == null) && !(Quantity == null))
             {
                 decimal total = 0;

                 Price = (decimal)Quantity * (decimal)Product.Price;
             }*/
        }
    }
}
