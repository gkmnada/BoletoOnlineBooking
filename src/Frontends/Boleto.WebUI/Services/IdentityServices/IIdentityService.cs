using Boleto.WebUI.Models;

namespace Boleto.WebUI.Services.IdentityServices
{
    public interface IIdentityService
    {
        Task<bool> SignInAsync(LoginModel model);
        Task<bool> GetRefreshTokenAsync();
    }
}
