using FreeCourse.Shared.Dtos;
using FreeCourse.Web.Models;
using FreeCourse.Web.ServicesContract;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;

namespace FreeCourse.Web.Services
{
    public class IdentityService : IIdentityService
    {
        private readonly HttpClient httpClient;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly ClientSettings clientSettings;
        private readonly ServiceApiSettings serviceApiSettings;

        public IdentityService(HttpClient httpClient, IHttpContextAccessor httpContextAccessor, IOptions<ClientSettings> clientSettings, IOptions<ServiceApiSettings> serviceApiSettings)
        {
            this.httpClient = httpClient;
            this.httpContextAccessor = httpContextAccessor;
            this.clientSettings = clientSettings.Value;
            this.serviceApiSettings = serviceApiSettings.Value;
        }

     
        /// <summary>
        /// Token süresi bittiginde cookideki refresh token bilgisi kullanılarak yeni bir access token alınır ve cookideki tokenlar güncellenir
        /// </summary>
        /// <returns></returns>
        public async Task<TokenResponse> GetAccessTokenByRefreshToken()
        {
            var discovery = await httpClient.GetDiscoveryDocumentAsync(new DiscoveryDocumentRequest
            {
                Address = serviceApiSettings.IdentityBaseUrl,
                Policy = { RequireHttps = false }
            });

            if (discovery.IsError)
            {
                throw discovery.Exception;
            }

            var refreshToken = await httpContextAccessor.HttpContext.GetTokenAsync(OpenIdConnectParameterNames.RefreshToken); // cookieden refresh token alındı

            var refreshTokenRequest = new RefreshTokenRequest
            {
                ClientId = clientSettings.WebClientForUser.ClientId,
                ClientSecret = clientSettings.WebClientForUser.ClientSecret,
                RefreshToken = refreshToken,
                Address = discovery.TokenEndpoint
            };

            var token = await httpClient.RequestRefreshTokenAsync(refreshTokenRequest);

            if (token.IsError)
            {
                throw token.Exception;
            }

            var authenticationProperties = new List<AuthenticationToken>
            {
                 new AuthenticationToken{Name =OpenIdConnectParameterNames.AccessToken,Value =token.AccessToken},
                 new AuthenticationToken{Name =OpenIdConnectParameterNames.RefreshToken,Value =token.RefreshToken},
                 new AuthenticationToken{Name =OpenIdConnectParameterNames.ExpiresIn,Value =DateTime.Now.AddSeconds(token.ExpiresIn).ToString("o",CultureInfo.InvariantCulture)},
            };

            var authenticationResult = await httpContextAccessor.HttpContext.AuthenticateAsync();
            authenticationResult.Properties.StoreTokens(authenticationProperties); // Yeni alınan token ile cookideki token değiştirir

            await httpContextAccessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, authenticationResult.Principal, authenticationResult.Properties);

            return token;
        }

        /// <summary>
        /// Kullanıcı çıkış yaptıgında Veritabanındaki refresh token siler
        /// </summary>
        /// <returns></returns>
        public async Task RevokeRefreshToken()
        {
            var discovery = await httpClient.GetDiscoveryDocumentAsync(new DiscoveryDocumentRequest
            {
                Address = serviceApiSettings.IdentityBaseUrl,
                Policy =  { RequireHttps = false }
            });

            if (discovery.IsError)
            {
                throw discovery.Exception;
            }

            var refreshToken = await httpContextAccessor.HttpContext.GetTokenAsync(OpenIdConnectParameterNames.RefreshToken);

            var tokenRevocationRequest = new TokenRevocationRequest
            {
                ClientId = clientSettings.WebClientForUser.ClientId,
                ClientSecret = clientSettings.WebClientForUser.ClientSecret,
                Address = discovery.RevocationEndpoint,
                Token = refreshToken,
                TokenTypeHint = "refresh_token"
            };

            await httpClient.RevokeTokenAsync(tokenRevocationRequest);
        }

        /// <summary>
        /// Kullanıcı giriş yaptığında  kullanıcıya ait claimleri,access token ,refresh token bilgilerini cookie ekler
        /// </summary>
        /// <returns></returns>
        public async Task<Response<bool>> SignIn(SignInInput signinInput)
        {
            var discovery = await httpClient.GetDiscoveryDocumentAsync(new DiscoveryDocumentRequest
            {
                Address = serviceApiSettings.IdentityBaseUrl,
                Policy =  { RequireHttps = false }
            });

            if (discovery.IsError)
            {
                throw discovery.Exception;
            }

            var passwordTokenRequest = new PasswordTokenRequest
            {
                ClientId = clientSettings.WebClientForUser.ClientId,
                ClientSecret = clientSettings.WebClientForUser.ClientSecret,
                UserName = signinInput.Email,
                Password = signinInput.Password,
                Address = discovery.TokenEndpoint
            };

            var token = await httpClient.RequestPasswordTokenAsync(passwordTokenRequest);

            if (token.IsError)
            {
                var responseContent = await token.HttpResponse.Content.ReadAsStringAsync();

                var errorDto = JsonSerializer.Deserialize<ErrorDto>(responseContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }); // deserilaze yaparken buyuk kucuk harf takılma

                return Response<bool>.Fail(errorDto.Errors, 400);
            }

            var userInfoRequest = new UserInfoRequest
            {
                Token = token.AccessToken,
                Address = discovery.UserInfoEndpoint,
            };

            var userInfo = await httpClient.GetUserInfoAsync(userInfoRequest);

            if (userInfo.IsError)
            {
                throw userInfo.Exception;
            }

            var claimsIdentity = new ClaimsIdentity(userInfo.Claims, CookieAuthenticationDefaults.AuthenticationScheme, "name", "role");

            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

            var authenticationProperties = new AuthenticationProperties();
            authenticationProperties.StoreTokens(new List<AuthenticationToken>
            {
                 new AuthenticationToken{Name =OpenIdConnectParameterNames.AccessToken,Value =token.AccessToken},
                 new AuthenticationToken{Name =OpenIdConnectParameterNames.RefreshToken,Value =token.RefreshToken},
                 new AuthenticationToken{Name =OpenIdConnectParameterNames.ExpiresIn,Value =DateTime.Now.AddSeconds(token.ExpiresIn).ToString("o",CultureInfo.InvariantCulture)},
            });

            authenticationProperties.IsPersistent = signinInput.Remember;

            await httpContextAccessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal, authenticationProperties);

            return Response<bool>.Success(200);
        }
    }
}
