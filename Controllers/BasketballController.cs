using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using YetAnotherDemo.Services;

namespace YetAnotherDemo.Controllers
{
    [Authorize(Roles = "Admin, Basketball")]
    public class BasketballController : Controller
    {
        private readonly BlobStorageService _blobStorageService;

        public BasketballController()
        {
            _blobStorageService = new BlobStorageService();
        }

        public IActionResult Index()
        {
            return View(new BlobViewModel
            {
                Containers = _blobStorageService.ListContainers()
            });
        }
    }

    public class BlobViewModel
    {
        public List<string> Containers { get; set; }
    }
}