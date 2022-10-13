﻿using IdentityServer_IdentityAPI.AuthServer.Models;
using IdentityServerIdentityAPI.AuthServer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
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
        private readonly UserManager<ApplicationUser> _userManager;

        public UserController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpPost]
        public async Task<IActionResult> SignUp(UserSaveViewModel userSaveViewModel)
        {
            ApplicationUser applicationUser = new ApplicationUser();

            applicationUser.UserName = userSaveViewModel.UserName;
            applicationUser.Email = userSaveViewModel.Email;
            applicationUser.City = userSaveViewModel.City;

            var result = await _userManager.CreateAsync(applicationUser, userSaveViewModel.Password);

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors.Select(x => x.Description));
            }

            return Ok("üye başarıyla kaydedildi");
        }
    }
}
