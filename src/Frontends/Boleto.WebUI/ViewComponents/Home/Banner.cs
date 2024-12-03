using Microsoft.AspNetCore.Mvc;

namespace Boleto.WebUI.ViewComponents.Home
{
    public class Banner : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
