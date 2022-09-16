﻿// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4;
using IdentityServer4.Models;
using System;
using System.Collections.Generic;

namespace FreeCourse.IdentityServer
{
    public static class Config
    {
        public static IEnumerable<ApiResource> ApiResources => new ApiResource[]
        {
            new ApiResource("resource_catalog"){Scopes={"catalog_fullpermission"}},
            new ApiResource("resource_photo_stock"){Scopes={"photo_stock_fullpermission"}},
            new ApiResource("resource_basket"){Scopes={"basket_fullpermission"}},
            new ApiResource("resource_discount"){Scopes={"discount_fullpermission"}},
            new ApiResource("resource_order"){Scopes={"order_fullpermission"}},
            new ApiResource("resource_payment"){Scopes={"payment_fullpermission"}},
            new ApiResource("resource_gateway"){Scopes={"gateway_fullpermission"}},
            new ApiResource(IdentityServerConstants.LocalApi.ScopeName){Scopes={IdentityServerConstants.LocalApi.ScopeName}}
        };

        public static IEnumerable<IdentityResource> IdentityResources =>
                   new IdentityResource[]
                   {
                       new IdentityResources.Email(), // claimleri hazır içinde var
                       new IdentityResources.OpenId(),
                       new IdentityResources.Profile(),
                       new IdentityResource(){Name="roles",DisplayName="Roles",Description="Kullanıcı Rolleri",UserClaims=new []{"role"}} // roles resource role claim ne maplendi
                    };

        public static IEnumerable<ApiScope> ApiScopes =>
            new ApiScope[]
            {
                new ApiScope("catalog_fullpermission","Catalog API için full erişim"),
                new ApiScope("photo_stock_fullpermission","Photo Stock API için full erişim"),
                new ApiScope("basket_fullpermission","Basket API için full erişim"),
                new ApiScope("discount_fullpermission","Discount API için full erişim"),
                new ApiScope("order_fullpermission","Order API için full erişim"),
                new ApiScope("payment_fullpermission","Payment API için full erişim"),
                new ApiScope("gateway_fullpermission","Gateway API için full erişim"),
                new ApiScope(IdentityServerConstants.LocalApi.ScopeName),
            };

        public static IEnumerable<Client> Clients =>
            new Client[]
            {
                new Client
                {
                    ClientId="WebMvcClient",
                    ClientSecrets = { new Secret("secret".Sha256())},
                    AllowedGrantTypes= {GrantType.ClientCredentials},
                    AllowedScopes={
                        "catalog_fullpermission",
                        "photo_stock_fullpermission", IdentityServerConstants.LocalApi.ScopeName,
                        "gateway_fullpermission",
                    }
                },
                new Client
                {
                    ClientId ="WebMvcClientForUser",
                    ClientSecrets={new Secret("secret".Sha256())},
                    AllowOfflineAccess=true, // Refresh token izin ver
                    AllowedGrantTypes={GrantType.ResourceOwnerPassword},
                    AllowedScopes ={ IdentityServerConstants.StandardScopes.Email,
                                     IdentityServerConstants.StandardScopes.OpenId,
                                     IdentityServerConstants.StandardScopes.Profile,
                                     IdentityServerConstants.StandardScopes.OfflineAccess, // Refresh Token 
                                     "roles",
                                     IdentityServerConstants.LocalApi.ScopeName,
                                     "basket_fullpermission",
                                     "order_fullpermission",
                                     "gateway_fullpermission"
                                    },
                    AccessTokenLifetime =60*60, // 1 saat
                    RefreshTokenExpiration =TokenExpiration.Absolute, // Refresh token süresi sabit 61 .günde refresh token geçerlş olmaz
                    AbsoluteRefreshTokenLifetime =(int)(DateTime.Now.AddDays(60)-DateTime.Now).TotalSeconds, // refresh token süresi 60 gün
                    RefreshTokenUsage = TokenUsage.ReUse // refresh token tekrar kullanılabilir.
                },
                new Client
                {
                    ClientName="Token Exchange Client",
                    ClientId="TokenExchangeClient",
                    ClientSecrets = { new Secret("secret".Sha256())},
                    AllowedGrantTypes=new []{"urn:ietf:params:oauth:grant-type:token-exchange"},
                    AllowedScopes={"discount_fullpermission",
                                   "payment_fullpermission",
                                   IdentityServerConstants.StandardScopes.OpenId}
                },
            };
    }
}