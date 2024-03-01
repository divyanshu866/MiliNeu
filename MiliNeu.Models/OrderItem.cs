using System.ComponentModel.DataAnnotations;

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


        // Nav Prop Right
        [Required]
        public Product Product { get; set; }
        public OrderItem()
        {
            DateTimeCreated = DateTime.UtcNow;
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
