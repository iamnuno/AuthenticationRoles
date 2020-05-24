using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Linq;
using YetAnotherDemo.Data;
using YetAnotherDemo.Models;

namespace YetAnotherDemo
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateWebHostBuilder(args).Build();

            using (var services = host.Services.CreateScope())
            {   

                // add initial roles and admin user to identity
                var dbContext = services.ServiceProvider.GetRequiredService<AppIdentityDbContext>();
                var userMgr = services.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
                var roleMgr = services.ServiceProvider.GetRequiredService<RoleManager<AppRole>>();

                dbContext.Database.Migrate();

                var adminRole = new AppRole
                {
                    Name = "Admin"
                };
                var basketballRole = new AppRole
                {
                    Name = "Basketball"
                };
                var iceHockeyRole = new AppRole
                {
                    Name = "Ice Hockey"
                };

                if (!dbContext.Roles.Any())
                {
                    roleMgr.CreateAsync(adminRole).GetAwaiter().GetResult();
                    roleMgr.CreateAsync(basketballRole).GetAwaiter().GetResult();
                    roleMgr.CreateAsync(iceHockeyRole).GetAwaiter().GetResult();
                }

                if (!dbContext.Users.Any(u => u.UserName == "admin"))
                {
                    var adminUser = new AppUser
                    {
                        UserName = "Admin",
                        Email = "admin@test.com",

                    };
                    var result = userMgr.CreateAsync(adminUser, "Password1").GetAwaiter().GetResult();
                    userMgr.AddToRoleAsync(adminUser, adminRole.Name).GetAwaiter().GetResult();
                }

                // add initial blob storage to emulator
                CloudStorageAccount storageAccount = CloudStorageAccount.DevelopmentStorageAccount;
                CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
                CloudBlobContainer basketballContainer = blobClient.GetContainerReference("basketball");
                CloudBlobContainer iceHockeyContainer = blobClient.GetContainerReference("icehockey");
                basketballContainer.CreateIfNotExists();
                iceHockeyContainer.CreateIfNotExists();
            }

            host.Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
           WebHost.CreateDefaultBuilder(args)
               .UseStartup<Startup>();
    }
}
