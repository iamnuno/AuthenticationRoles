using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YetAnotherDemo.Models
{
    public class AppUser : IdentityUser<int>
    {
        public string Address { get; set; }
    }
}
