using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using YetAnotherDemo.Models;

namespace YetAnotherDemo.Data
{
    public class AppIdentityDbContext : IdentityDbContext<AppUser, AppRole, int>
    {
        public AppIdentityDbContext(DbContextOptions<AppIdentityDbContext> options)
            : base(options)
        {
        }
    }
}
