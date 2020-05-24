using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using YetAnotherDemo.Models;
using YetAnotherDemo.Services;

namespace YetAnotherDemo.Controllers
{
    [Authorize(Roles = "Admin, Basketball")]
    public class BasketballController : Controller
    {
        private readonly BlobStorageService _blobStorageService;
        private readonly UserManager<AppUser> _userManager;

        public BasketballController(UserManager<AppUser> userManager)
        {
            _blobStorageService = new BlobStorageService();
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UploadFile(BlobStorageModels model)
        {

            var user = await _userManager.GetUserAsync(HttpContext.User);
            model.File.Owner = user.UserName;
            await _blobStorageService.UploadFile("basketball", model);

            return View("Index");
        }

        [HttpPost]
        public IActionResult ListFiles(BlobStorageModels model)
        {

            var files = _blobStorageService.SearchFiles("basketball", model.SearchTerm.Search);

            BlobStorageModels newModel = new BlobStorageModels();
            newModel.ListFileTableEntity.FileTableEntitiesList = files;

            return View("Index", newModel);
        }

        public async Task<IActionResult> DeleteFile(BlobStorageModels model)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            _blobStorageService.DeleteFile("basketball", model.FileDelete.ID, user.UserName);
            return View("Index");
        }

    }
}