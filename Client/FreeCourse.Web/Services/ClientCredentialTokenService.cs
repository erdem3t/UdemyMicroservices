using FreeCourse.Web.Models;
using FreeCourse.Web.ServicesContract;
using IdentityModel.AspNetCore.AccessTokenManagement;
using IdentityModel.Client;
using Microsoft.Extensions.Options;
using System.Net.Http;
using System.Threading.Tasks;

namespace FreeCourse.Web.Services
{
    public class ClientCredentialTokenService : IClientCredentialTokenService
    {
        private readonly ServiceApiSettings serviceApiSettings;
        private readonly ClientSettings clientSettings;
        private readonly IClientAccessTokenCache clientAccessTokenCache;
        private readonly HttpClient httpClient;

        public ClientCredentialTokenService(IOptions<ServiceApiSettings> serviceApiSettings, IOptions<ClientSettings> clientSettings, IClientAccessTokenCache clientAccessTokenCache, HttpClient httpClient)
        {
            this.serviceApiSettings = serviceApiSettings.Value;
            this.clientSettings = clientSettings.Value;
            this.clientAccessTokenCache = clientAccessTokenCache;
            this.httpClient = httpClient;
        }

        /// <summary>
        /// Client Credential tokenı cache de yoksa cache yaz aksi taktirde cache dekini döndür.
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetToken()
        {
            var currentToken = await clientAccessTokenCache.GetAsync("WebClientToken");

            if (currentToken != null)
            {
                return currentToken.AccessToken;
            }

            var discovery = await httpClient.GetDiscoveryDocumentAsync(new DiscoveryDocumentRequest
            {
                Address = serviceApiSettings.IdentityBaseUrl,
                Policy = { RequireHttps = false }
            });

            if (discovery.IsError)
            {
                throw discovery.Exception;
            }

            var clientCridentialTokenRequest = new ClientCredentialsTokenRequest
            {
                ClientId = clientSettings.WebClient.ClientId,
                ClientSecret = clientSettings.WebClient.ClientSecret,
                Address = discovery.TokenEndpoint,
            };

            var token = await httpClient.RequestClientCredentialsTokenAsync(clientCridentialTokenRequest);

            if (token.IsError)
            {
                throw token.Exception;
            }

            await clientAccessTokenCache.SetAsync("WebClientToken", token.AccessToken, token.ExpiresIn);

            return token.AccessToken;
        }
    }
}
