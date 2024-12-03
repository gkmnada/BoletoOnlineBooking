using Microsoft.AspNetCore.Mvc;

namespace Boleto.WebUI.ViewComponents.Home
{
    public class Search : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
