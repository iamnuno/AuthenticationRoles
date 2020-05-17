using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace YetAnotherDemo.Controllers
{
    [Authorize(Roles = "Admin, Ice Hockey")]
    public class IceHockeyController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}