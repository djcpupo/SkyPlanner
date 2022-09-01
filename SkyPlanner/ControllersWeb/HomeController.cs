using Microsoft.AspNetCore.Mvc;

namespace SkyPlanner.ControllersWeb
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}