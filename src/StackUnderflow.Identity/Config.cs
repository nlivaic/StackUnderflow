// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4;
using IdentityServer4.Models;
using System.Collections.Generic;

namespace StackUnderflow.Identity
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> IdentityResources =>
            new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResource(
                    "role",
                    "Roles",
                    new string[] { "role" })
            };

        public static IEnumerable<ApiScope> ApiScopes =>
            new ApiScope[]
            {
                new ApiScope(
                    "stack_underflow_api",
                    "Stack Underflow Api",
                    new List<string> { "role" })
                };

        public static IEnumerable<ApiResource> ApiResources =>
            new ApiResource[]
            {
                new ApiResource
                {
                    Name = "stack_underflow_api",
                    DisplayName = "Stack Underflow Api",
                    Scopes = new List<string> {"stack_underflow_api"},
                    UserClaims = new List<string> { "role" },
                    ApiSecrets = new List<Secret> { new Secret("stack_underflow_api_secret".Sha256()) }
                }
             };

        public static IEnumerable<Client> Clients =>
            new Client[]
            {
                new Client
                {
                    ClientName = "StackUnderflowClient",
                    ClientId = "stack_underflow_client",        // @nl: create a random name.
                    AllowedGrantTypes = GrantTypes.Code,
                    RequirePkce = true,
                    RequireClientSecret = false,
                    AllowAccessTokensViaBrowser = true,
                    AccessTokenType = AccessTokenType.Jwt,
                    RedirectUris = { "http://localhost:3000/signin-oidc", "http://localhost:3000/signin-silent-oidc" },
                    PostLogoutRedirectUris = { "http://localhost:3000/signout-callback-oidc" },
                    AllowOfflineAccess = false,  // Do not allow refresh tokens
                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "stack_underflow_api"
                    },
                    ClientSecrets = { new Secret("secret".Sha256()) },
                    AccessTokenLifetime = 120       // Dev only   
                }
            };
    }
}