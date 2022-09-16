using FreeCourse.IdentityServer.Models;
using IdentityModel;
using IdentityServer4.Validation;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FreeCourse.IdentityServer.Services
{
    public class IdentityResourceOwnerPasswordValidator : IResourceOwnerPasswordValidator
    {
        private readonly UserManager<ApplicationUser> userManager;

        public IdentityResourceOwnerPasswordValidator(UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
        }

        public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            var user = await userManager.FindByEmailAsync(context.UserName);

            if (user == null)
            {

                context.Result.CustomResponse = new Dictionary<string, object>
                {
                    {"Errors",new List<string>{"Email veya şifre bulunamadı "}}
                };

                return;
            }

            var passwordCheck = await userManager.CheckPasswordAsync(user, context.Password);

            if (!passwordCheck)
            {

                context.Result.CustomResponse = new Dictionary<string, object>
                {
                    {"Errors",new List<string>{"Email veya şifre bulunamadı "}}
                };

                return;
            }

            context.Result = new GrantValidationResult(user.Id.ToString(), OidcConstants.AuthenticationMethods.Password); // Yeni bir token üretir
        }
    }
}
