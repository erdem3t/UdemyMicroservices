using IdentityModel.Client;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace FreeCourse.Gateway.Handler
{
    public class TokenExchangeDelegateHandler : DelegatingHandler
    {
        private readonly HttpClient httpClient;
        private readonly IConfiguration configuration;
        private string accessToken;

        public TokenExchangeDelegateHandler(HttpClient httpClient, IConfiguration configuration)
        {
            this.httpClient = httpClient;
            this.configuration = configuration;
        }

        private async Task<string> GetToken(string requestToken)
        {
            if (!string.IsNullOrEmpty(accessToken))
                return accessToken;

            var discovery = await httpClient.GetDiscoveryDocumentAsync(new DiscoveryDocumentRequest
            {
                Address = configuration["IdentityServerUrl"],
                Policy =  { RequireHttps = false }
            });

            if (discovery.IsError)
            {
                throw discovery.Exception;
            }

            var tokenExchangeTokenRequest = new TokenExchangeTokenRequest
            {
                Address = discovery.TokenEndpoint,
                ClientId = configuration["ClientId"],
                ClientSecret = configuration["ClientSecret"],
                GrantType = configuration["GrantType"],
                SubjectToken = requestToken,
                SubjectTokenType = "urn:ietf:params:oauth:token-type:access-token",
                Scope = "openid payment_fullpermission discount_fullpermission",
            };

            var tokenResponse = await httpClient.RequestTokenExchangeTokenAsync(tokenExchangeTokenRequest);

            if (tokenResponse.IsError)
            {
                throw tokenResponse.Exception;
            }

            accessToken = tokenResponse.AccessToken;

            return accessToken;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var requestToken = request.Headers.Authorization.Parameter;
            var exchangeToken = await GetToken(requestToken);
            request.SetBearerToken(exchangeToken);

            return await base.SendAsync(request, cancellationToken);
        }
    }
}
