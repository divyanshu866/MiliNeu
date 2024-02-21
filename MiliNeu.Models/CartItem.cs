using System.ComponentModel.DataAnnotations;

namespace MiliNeu.Models
{
    public class CartItem
    {
       
        public int Id { get; set; }
        public int CartId { get; set; }
        public int ProductId { get; set; }
        public string SelectedSize { get; set; }
        public int Quantity { get; set; }

        // Nav Prop Left
        public Cart Cart { get; set; }

        // Nav Prop Right
        public Product Product { get; set; }
    }
}
