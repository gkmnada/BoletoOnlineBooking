using Microsoft.AspNetCore.Mvc;

namespace Boleto.WebUI.Areas.Administration.ViewComponents.AdminLayout
{
    public class AdminHeader : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
