using IdentityModel;
using IdentityServer_IdentityAPI.AuthServer.Models;
using IdentityServer4.Validation;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServerIdentityAPI.AuthServer.Service
{
    public class IdentityResourceOwnerPasswordValidator : IResourceOwnerPasswordValidator
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public IdentityResourceOwnerPasswordValidator(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            var existUser = await _userManager.FindByEmailAsync(context.UserName);

            if (existUser == null) return;

            var passwordCheck = await _userManager.CheckPasswordAsync(existUser, context.Password);

            if (passwordCheck == false) return;

            context.Result = new GrantValidationResult(existUser.Id.ToString(), OidcConstants.AuthenticationMethods.Password);
            //Buradan sonra bu kısmın aktifleştirmek için startup tarafından da eklenir. AddResourceOwnerValidator<IdentityResourceOwnerPasswordValidator>() daha fazla bilgi için IdentiyServer4 74. Ders

        }
    }
}
