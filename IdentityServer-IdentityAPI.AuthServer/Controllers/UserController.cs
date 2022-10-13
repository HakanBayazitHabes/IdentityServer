using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static IdentityServer4.IdentityServerConstants;

namespace IdentityServerIdentityAPI.AuthServer.Controllers
{
    [Route("api/[controller]/[action]")]
    [Authorize(LocalApi.PolicyName)] //Claim bazlı yetkilendirme yapıyoruz. Buraya Gelen Client'da Bu izni ApiResources VeScopelarında tanımlamıştık , bu policyname nerden geliyor bunu startup kısında eklemiş olduğumuz services.AddLocalApiAuthentication() 'den geliyor,otomatik olarak
    [ApiController]
    public class UserController : ControllerBase
    {
        [HttpPost]
        public IActionResult SignUp()
        {
            return Ok("signup çalıştı");
        }
    }
}
