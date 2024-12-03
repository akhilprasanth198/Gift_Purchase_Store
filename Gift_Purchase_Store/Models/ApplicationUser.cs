using Microsoft.AspNetCore.Identity;

namespace Gift_Purchase_Store.Models
{
    public class ApplicationUser : IdentityUser
    {
        public ICollection<Order>? Orders { get; set; }
    }
}
