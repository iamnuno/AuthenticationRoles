using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace YetAnotherDemo.Controllers
{
    [Authorize(Roles = "Admin, Basketball")]
    public class BasketballController : Controller
    {
        
        public IActionResult Index()
        {
            return View();
        }
    }
}