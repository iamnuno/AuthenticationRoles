using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using YetAnotherDemo.Data;
using YetAnotherDemo.Models;

namespace YetAnotherDemo.Controllers
{

    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly AppIdentityDbContext _context;
        private readonly RoleManager<AppRole> _roleManager;

        public AdminController(
            AppIdentityDbContext context,
            UserManager<AppUser> userManager,
            RoleManager<AppRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> CreateRole(AdminViewModel model)
        {

            if (!_context.Roles.Any(r => r.Name == model.Role.RoleName))
            {
                var role = new AppRole 
                { 
                    Name = model.Role.RoleName
                };

                var result = await _roleManager.CreateAsync(role);

                if (result.Succeeded)
                {
                    ViewBag.Message1 = "Role created";
                    
                }
                else
                {
                    ViewBag.Message1 = "Role not created";
                }

            } else
            {
                ViewBag.Message1 = "Role already exists";
            }
           
            return View("Index");
        }

        public async Task<IActionResult> AddRoleToUser(AdminViewModel model)
        {
            if (_context.Roles.Any(r => r.Name == model.Role.RoleName) 
                && _context.Users.Any(u => u.UserName == model.AccountStatus.AccountName))
            {

                var user = _context.Users
                    .Where(u => u.UserName == model.AccountStatus.AccountName)
                    .Single(); // assuming no duplicated names...

                var result = await _userManager.AddToRoleAsync(user, model.Role.RoleName);

                if (result.Succeeded)
                {
                    ViewBag.Message2 = "Role added to user";

                }
                else
                {
                    ViewBag.Message2 = "Role not added to user";
                }

            }
            else
            {
                ViewBag.Message2 = "Role/user does not exist";
            }

            return View("Index");
            
        }

        public async Task<IActionResult> UpdateAccountLockStatus(AdminViewModel model)
        {
            var user = _context.Users.Where(u => u.UserName == model.AccountStatus.AccountName).Single(); 

            if (user != null)
            {

                var lockOut = user.LockoutEnd;

                if (lockOut == null)
                {
                    await _userManager.SetLockoutEndDateAsync(user, new DateTime(2999, 01, 01));
                    ViewBag.Message3 = "User account locked";
                } else
                {
                    await _userManager.SetLockoutEndDateAsync(user, null);
                    ViewBag.Message3 = "User account unlocked";
                }

            }
            return View("Index");
        }


    }
}