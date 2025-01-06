
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MiliNeu.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        public int CartId { get; set; }

        //Nav Prop Right
        [Required]
        [ForeignKey("CartId")]
        public Cart Cart { get; set; }

        //Nav Prop Right

        public List<Order>? UserOrders { get; set; }
        public ICollection<Address>? Addresses { get; set; }
        public List<OrderIssue>? OrderIssues { get; set; }
        public ApplicationUser()
        {
            UserOrders = new List<Order>();
        }
    }
}
