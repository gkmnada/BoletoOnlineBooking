using Boleto.WebUI.Models;
using Boleto.WebUI.Settings;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System.Globalization;
using System.Security.Claims;

namespace Boleto.WebUI.Services.IdentityServices
{
    public class IdentityService : IIdentityService
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ApiSettings _apiSettings;
        private readonly ClientSettings _clientSettings;

        public IdentityService(HttpClient httpClient, IHttpContextAccessor httpContextAccessor, IOptions<ApiSettings> apiSettings, IOptions<ClientSettings> clientSettings)
        {
            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;
            _apiSettings = apiSettings.Value;
            _clientSettings = clientSettings.Value;
        }

        public async Task<bool> GetRefreshTokenAsync()
        {
            var discovery = await _httpClient.GetDiscoveryDocumentAsync(new DiscoveryDocumentRequest
            {
                Address = _apiSettings.IdentityUri,
            });

            var refreshToken = await _httpContextAccessor.HttpContext.GetTokenAsync(OpenIdConnectParameterNames.RefreshToken);

            var tokenResponse = await _httpClient.RequestRefreshTokenAsync(new RefreshTokenRequest
            {
                Address = discovery.TokenEndpoint,
                ClientId = _clientSettings.ClientId,
                ClientSecret = _clientSettings.ClientSecret,
                GrantType = OpenIdConnectGrantTypes.RefreshToken,
                RefreshToken = refreshToken,
            });

            if (tokenResponse.IsError)
            {
                await _httpContextAccessor.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                return false;
            }

            var authenticationTokens = new List<AuthenticationToken>
            {
                new AuthenticationToken
                {
                    Name = OpenIdConnectParameterNames.AccessToken,
                    Value = tokenResponse.AccessToken,
                },
                new AuthenticationToken
                {
                    Name = OpenIdConnectParameterNames.RefreshToken,
                    Value = tokenResponse.RefreshToken,
                },
                new AuthenticationToken
                {
                    Name = OpenIdConnectParameterNames.ExpiresIn,
                    Value = DateTime.UtcNow.AddSeconds(tokenResponse.ExpiresIn).ToString("o", CultureInfo.InvariantCulture),
                }
            };

            var authenticateResult = await _httpContextAccessor.HttpContext.AuthenticateAsync();
            authenticateResult.Properties.StoreTokens(authenticationTokens);

            await _httpContextAccessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, authenticateResult.Principal, authenticateResult.Properties);

            return true;
        }

        public async Task<bool> SignInAsync(LoginModel model)
        {
            var discovery = await _httpClient.GetDiscoveryDocumentAsync(new DiscoveryDocumentRequest
            {
                Address = _apiSettings.IdentityUri,
            });

            var tokenResponse = await _httpClient.RequestPasswordTokenAsync(new PasswordTokenRequest
            {
                Address = discovery.TokenEndpoint,
                ClientId = _clientSettings.ClientId,
                ClientSecret = _clientSettings.ClientSecret,
                UserName = model.Username,
                Password = model.Password
            });

            var userInfo = await _httpClient.GetUserInfoAsync(new UserInfoRequest
            {
                Address = discovery.UserInfoEndpoint,
                Token = tokenResponse.AccessToken
            });

            var claimsIdentity = new ClaimsIdentity(userInfo.Claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

            var authenticationProperties = new AuthenticationProperties
            {
                IsPersistent = false,
            };

            authenticationProperties.StoreTokens(new List<AuthenticationToken>()
            {
                new AuthenticationToken
                {
                    Name = OpenIdConnectParameterNames.AccessToken,
                    Value = tokenResponse.AccessToken
                },
                new AuthenticationToken
                {
                    Name = OpenIdConnectParameterNames.RefreshToken,
                    Value = tokenResponse.RefreshToken
                },
                new AuthenticationToken
                {
                    Name = OpenIdConnectParameterNames.ExpiresIn,
                    Value = DateTime.UtcNow.AddSeconds(tokenResponse.ExpiresIn).ToString("o", CultureInfo.InvariantCulture)
                },
            });

            await _httpContextAccessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal, authenticationProperties);

            return true;
        }
    }
}
