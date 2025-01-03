using Microsoft.AspNetCore.Mvc;

namespace Boleto.WebUI.Areas.Administration.ViewComponents.AdminLayout
{
    public class AdminSidebar : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
