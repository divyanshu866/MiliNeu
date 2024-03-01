using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace MiliNeu.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        public int CartId { get; set; }

        //Nav Prop Right
        [Required]
        public Cart Cart { get; set; }

        //Nav Prop Right
        public List<Order>? UserOrders { get; set; }

        public ApplicationUser()
        {
            UserOrders = new List<Order>();
        }
    }
}
