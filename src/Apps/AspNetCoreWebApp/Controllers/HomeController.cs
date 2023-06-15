using AspNetCoreWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Practice.Foundation.Infrastructure.Types;
using System.Diagnostics;

namespace AspNetCoreWebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            List<BaseItem> Items=new List<BaseItem>();
            BaseItem baseItem1 = new BaseItem("Demo1");
            BaseItem baseItem2 = new BaseItem("Demo2");
            BaseItem baseItem3 = new BaseItem("Demo3");
            baseItem1.SetLanguageField("Name", "Nishchay");
            baseItem2.SetLanguageField("Name", "Hoshima");
            baseItem3.SetLanguageField("Name", "Vrinda");
            baseItem1.SetLanguageField("successfull", "TRUE");
            baseItem2.SetLanguageField("successfull", "TRUE");
            baseItem3.SetLanguageField("successfull", "TRUE");
            Items.Add(baseItem1);
            Items.Add(baseItem2);
            Items.Add(baseItem3);
            return View(Items);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}