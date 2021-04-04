using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UdemyIdentityServer.API1.Model;

namespace UdemyIdentityServer.API1.Controllers
{    
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        //api/products
        [Authorize(Policy = "ReadProduct")]
        [HttpGet]
        public IActionResult GetProducts()
        {
            var productList = new List<Product>()
            {
                new Product(){Id=1,Name = "Kalem",Price = 100,Stock =500 },
                new Product(){Id=2,Name = "Silgi",Price = 120,Stock =100 },
                new Product(){Id=2,Name = "Çanta",Price = 1200,Stock =300 },

            };
            return Ok(productList);
        }

        [Authorize(Policy="UpdateOrCreate")]
        public IActionResult UpdateProduct(int id)
        {
            return Ok($"Id'si {id} olan ürün güncellenmiştir.");
        }
        [Authorize(Policy = "UpdateOrCreate")]
        public IActionResult CreateProduct(Product product)
        {
            return Ok(product);
        }

    }


    }
