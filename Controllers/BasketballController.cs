using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using YetAnotherDemo.Models;
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
            return View();
        }


        public IActionResult UploadFile(FileUploadModel model)
        {
            var file = model.File;
            _blobStorageService.UploadFile("basketball", file);

            return View("Index");
        }


    }




    /*

    public class BlobViewModel
    {
        public List<string> Containers { get; set; }
    }

    */
}