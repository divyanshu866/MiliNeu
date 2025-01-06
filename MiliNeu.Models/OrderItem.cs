using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MiliNeu.Models
{
    public class OrderItem
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string SelectedSize { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        public DateTime DateTimeCreated { get; set; }

        [Required]
        public decimal Price { get; set; }
        public decimal? DiscountedPrice { get; set; } // Price after discount, if applicable


        // Nav Prop Right
        public int ProductId { get; set; }
        [Required]
        [ForeignKey("ProductId")]
        public Product Product { get; set; }

        public int ProductVariantId { get; set; }
        [Required]
        [ForeignKey("ProductVariantId")]
        public ProductVariant ProductVariant { get; set; }
        public OrderItem()
        {
            DateTimeCreated = DateTime.UtcNow;
        }
        public void calculatePrice()
        {
            /*if (!(Product == null) && !(Quantity == null))
            {
                decimal total = 0;

                Price = (decimal)Quantity * (decimal)Product.Price;
            }*/
        }
    }
}
