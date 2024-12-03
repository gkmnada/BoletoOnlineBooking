using Boleto.WebUI.Models;
using Boleto.WebUI.Services.IdentityServices;
using Microsoft.AspNetCore.Mvc;

namespace Boleto.WebUI.Controllers
{
    public class AccountController : Controller
    {
        private readonly IIdentityService _identityService;

        public AccountController(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model)
        {
            var response = await _identityService.SignInAsync(model);

            if (response == true)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }
    }
}
