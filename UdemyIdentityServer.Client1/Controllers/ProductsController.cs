using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Newtonsoft.Json;
using UdemyIdentityServer.API1.Model;
using UdemyIdentityServer.Client1.Services;

namespace UdemyIdentityServer.Client1.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IApiResourceHttpClient _apiResourceHttpClient;

        public ProductsController(IConfiguration configuration, IApiResourceHttpClient apiResourceHttpClient)
        {
            _configuration = configuration;
            _apiResourceHttpClient = apiResourceHttpClient;
        }
        public async Task<IActionResult> Index()
        {
            HttpClient httpClient = await _apiResourceHttpClient.GetHttpClient();

            var accesToken = await HttpContext.GetTokenAsync(OpenIdConnectParameterNames.AccessToken);

        
            // https://localhost:5006

            httpClient.SetBearerToken(accesToken);

            var response = await httpClient.GetAsync("https://localhost:5016/api/products/getproducts");
            List<Product> products = null;
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();

                 products = JsonConvert.DeserializeObject<List<Product>>(content);
            }
            else
            {
                //loglama 
            }

            return View(products);
        }
    }
}
