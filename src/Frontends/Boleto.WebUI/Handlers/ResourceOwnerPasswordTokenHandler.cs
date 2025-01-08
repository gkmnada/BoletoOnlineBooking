using Boleto.WebUI.Services.IdentityServices;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System.Net;
using System.Net.Http.Headers;

namespace Boleto.WebUI.Handlers
{
    public class ResourceOwnerPasswordTokenHandler : DelegatingHandler
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IIdentityService _identityService;

        public ResourceOwnerPasswordTokenHandler(IHttpContextAccessor httpContextAccessor, IIdentityService identityService)
        {
            _httpContextAccessor = httpContextAccessor;
            _identityService = identityService;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var token = await _httpContextAccessor.HttpContext.GetTokenAsync(OpenIdConnectParameterNames.AccessToken);

            if (!string.IsNullOrWhiteSpace(token))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            var response = await base.SendAsync(request, cancellationToken);

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                var tokenResponse = await _identityService.GetRefreshTokenAsync();

                if (tokenResponse)
                {
                    token = await _httpContextAccessor.HttpContext.GetTokenAsync(OpenIdConnectParameterNames.AccessToken);

                    if (!string.IsNullOrWhiteSpace(token))
                    {
                        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                        response = await base.SendAsync(request, cancellationToken);
                    }
                }

                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    await _httpContextAccessor.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                    _httpContextAccessor.HttpContext.Response.Redirect("/Account/Login");
                    await _httpContextAccessor.HttpContext.Response.CompleteAsync();
                }
            }

            return response;
        }
    }
}
