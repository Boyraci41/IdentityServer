using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace UdemyIdentityServer.Client1.Controllers
{
    public class UserController : Controller
    {
        private readonly IConfiguration _configuration;

        public UserController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task  LogOut()
        {
            await HttpContext.SignOutAsync("Cookies"); // Client Tarafından Çıkış
            await HttpContext.SignOutAsync("oidc"); //oidc yani auth serverdan çıkış
        }

        public async Task<IActionResult> GetRefreshToken()
        {

            HttpClient httpClientDisco = new HttpClient();

            var disco = await httpClientDisco.GetDiscoveryDocumentAsync("https://localhost:5001"); //discovery endpoint
            if (disco.IsError)
            {
                //Loglama yap
            }
            var refreshToken = await HttpContext.GetTokenAsync(OpenIdConnectParameterNames.RefreshToken);

            RefreshTokenRequest refreshTokenRequest = new RefreshTokenRequest();

            refreshTokenRequest.ClientId = _configuration["Client1Mvc:ClientId"];
            refreshTokenRequest.ClientSecret = _configuration["Client1Mvc:ClientSecret"];
            refreshTokenRequest.RefreshToken = refreshToken;
            refreshTokenRequest.Address = disco.TokenEndpoint;

            HttpClient httpClient = new HttpClient();
            var token = await httpClient.RequestRefreshTokenAsync(refreshTokenRequest);

            if (token.IsError)
            {
                //yönledirme yap
            }
            var tokens = new List<AuthenticationToken>()
            {
                new AuthenticationToken{Name= OpenIdConnectParameterNames.IdToken,Value = token.IdentityToken},
                new AuthenticationToken{Name= OpenIdConnectParameterNames.AccessToken,Value = token.RefreshToken},
                new AuthenticationToken{Name= OpenIdConnectParameterNames.RefreshToken,Value = token.RefreshToken},
                new AuthenticationToken{Name= OpenIdConnectParameterNames.ExpiresIn,Value = DateTime.UtcNow.AddSeconds(token.ExpiresIn).ToString("o",CultureInfo.InvariantCulture)},

            };

            var authenticationResult = await HttpContext.AuthenticateAsync();
            var properties = authenticationResult.Properties;

            properties.StoreTokens(tokens);

            await HttpContext.SignInAsync("Cookies", authenticationResult.Principal, properties);


            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Admin")]
        public IActionResult AdminAction()
        {
            return View();
        }
    }

    
}
