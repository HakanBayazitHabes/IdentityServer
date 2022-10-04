using IdentityServer.API2.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer.API2.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class PicturesController : ControllerBase
    {
        [Authorize]
        [HttpGet]
        public IActionResult GetPictures()
        {
            var pictures = new List<Picture>
            {
                new Picture{Id=1,Name="Doğa resmi",Url="dogaresmi.jpg"},
                new Picture{Id=2,Name="Manzara resmi",Url="dogaresmi.jpg"},
                new Picture{Id=3,Name="Kır resmi",Url="dogaresmi.jpg"},
                new Picture{Id=4,Name="Güneş resmi",Url="dogaresmi.jpg"},
                new Picture{Id=5,Name="Boz resmi",Url="dogaresmi.jpg"},
                new Picture{Id=6,Name="Bahar resmi",Url="dogaresmi.jpg"}
            };

            return Ok(pictures);
        }
    }
}
