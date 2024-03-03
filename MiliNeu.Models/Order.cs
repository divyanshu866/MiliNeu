using System.ComponentModel.DataAnnotations;

namespace MiliNeu.Models
{
    public class Order
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        public DateTime DateTimeCreated { get; set; }

        [Required]
        public bool PaymentStatus { get; set; }
        //Nav prop left

        [Required]
        public ApplicationUser ApplicationUser { get; set; }

        [Required]
        public List<OrderItem> Items { get; set; }


        public Order()
        {
            DateTimeCreated = DateTime.UtcNow;
            PaymentStatus = false;
        }
        public void calculatePrice()
        {
            if (!(Items == null))
            {
                decimal total = 0;
                foreach (var cartItem in Items)
                {
                    total += (decimal)cartItem.Price;
                }
                Price = total;
            }
        }
    }

}
