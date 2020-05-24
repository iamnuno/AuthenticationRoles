using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using YetAnotherDemo.Data;
using YetAnotherDemo.Models;

namespace YetAnotherDemo.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly AppIdentityDbContext _context;
        private readonly SignInManager<AppUser> _signInManager;

        public AccountController(
            AppIdentityDbContext context,
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }

        public IActionResult Register()
        {
            var roles = _context.Roles.ToList();
            return View(new AccountViewModel
            {
                Roles = roles.Where(x => x.Name != "Admin").Select(x => x.NormalizedName).ToList()
            });
        }

        [HttpPost]
        public async Task<IActionResult> Register(AccountViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (model.Account.Password.Equals(model.Account.ConfirmPassword))
                    {
                        if (!_context.Users.Any(u => u.Email == model.Account.Email))
                        {
                            var user = new AppUser
                            {
                                UserName = model.Account.Username,
                                Email = model.Account.Email,
                                PhoneNumber = model.Account.Phone,
                                Address = model.Account.Address
                            };

                            await _userManager.CreateAsync(user, model.Account.Password);
                            await _userManager.AddToRoleAsync(user, model.Account.Role);

                            return RedirectToAction("Index", "Home");
                        }
                        else
                        {
                            ViewBag.Message = "Email is already registered";
                            return View(model);
                        }
                    }
                    else
                    {
                        ViewBag.Message = "Passwords must match";
                        return View(model);
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
            }

            var roles = _context.Roles.ToList();
            return View(new AccountViewModel
            {
                Roles = roles.Where(x => x.Name != "Admin").Select(x => x.NormalizedName).ToList()
            });

        }


        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(AccountViewModel model)
        {

            var result = await _signInManager.PasswordSignInAsync(model.Account.Username, model.Account.Password, false, false);

            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }

            ViewBag.Message = "Sorry, try again";
            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(AccountViewModel model)
        {   
            if (model.Account.Password.Equals(model.Account.ConfirmPassword))
            {
                var user = await _userManager.GetUserAsync(HttpContext.User);

                if (user != null)
                {
                    user.PasswordHash = _userManager.PasswordHasher.HashPassword(user, model.Account.Password);
                    var result = await _userManager.UpdateAsync(user);

                    if (result.Succeeded)
                    {
                        ViewBag.Message = "Password updated";
                    }
                    else
                    {
                        ViewBag.Message = "There was a problem";
                    }
                }
            } else
            {
                ViewBag.Message = "Passwords don't match";
            }
           

            return View();
        }

        public IActionResult AccessDenied()
        {
            return View("_AccessDenied");
        }
    }
}