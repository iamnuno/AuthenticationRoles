using Microsoft.AspNetCore.Identity;

namespace YetAnotherDemo.Models
{
    public class AppUser : IdentityUser<int>
    {
        public string Address { get; set; }
    }
}
