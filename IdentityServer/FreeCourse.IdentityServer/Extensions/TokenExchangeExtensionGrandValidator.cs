using IdentityServer4.Models;
using IdentityServer4.Validation;
using System.Linq;
using System.Threading.Tasks;

namespace FreeCourse.IdentityServer.Extensions
{
    public class TokenExchangeExtensionGrandValidator : IExtensionGrantValidator
    {
        public string GrantType => "urn:ietf:params:oauth:grant-type:token-exchange";

        private readonly ITokenValidator tokenValidator;

        public TokenExchangeExtensionGrandValidator(ITokenValidator tokenValidator)
        {
            this.tokenValidator = tokenValidator;
        }

        public async Task ValidateAsync(ExtensionGrantValidationContext context)
        {
            var token = context.Request.Raw.Get("subject_token");

            if (string.IsNullOrEmpty(token))
            {
                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidRequest, "Token Missing");
                return;
            }

            var tokenValidationResult = await tokenValidator.ValidateAccessTokenAsync(token);

            if (tokenValidationResult.IsError)
            {
                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, "Token Invalid");
                return;
            }

            var subjectClaim = tokenValidationResult.Claims.FirstOrDefault(c => c.Type == "sub");

            if (subjectClaim == null)
            {
                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, "Token must contain sub value");
                return;
            }

            context.Result = new GrantValidationResult(subjectClaim.Value,"access_token",tokenValidationResult.Claims);
        }
    }
}
