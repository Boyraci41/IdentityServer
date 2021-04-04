using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using UdemyIdentityServer.API2.Models;

namespace UdemyIdentityServer.API2.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class PicturesController : ControllerBase
    {   
        [Authorize]
        [HttpGet]
        public IActionResult GetPictures()
        {
            var pictures=  new List<Picture>()
            {
                new Picture(){Id = 1,Name = "Doğa",Url = "Doğa.jpg"},
                new Picture(){Id = 2,Name = "Aslan",Url = "Aslan.jpg"},
                new Picture(){Id = 1,Name = "Fare",Url = "Fare.jpg"},
            };

            return Ok(pictures);
        }
    }
}
