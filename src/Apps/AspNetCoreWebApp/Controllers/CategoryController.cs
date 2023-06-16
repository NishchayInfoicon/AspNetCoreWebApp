using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreWebApp.Controllers
{
    public class CategoryController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
