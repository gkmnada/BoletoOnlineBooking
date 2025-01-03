using Microsoft.AspNetCore.Mvc;

namespace Boleto.WebUI.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
