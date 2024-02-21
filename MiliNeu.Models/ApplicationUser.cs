using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace MiliNeu.Models
{
    public class ApplicationUser : IdentityUser
    {
        
        public int CartId { get; set; }

        //Nav Prop Right
        public Cart Cart { get; set; }

    }
}
