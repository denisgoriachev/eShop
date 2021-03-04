// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4;
using IdentityServer4.Models;
using System.Collections.Generic;

namespace eShop.Auth
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> IdentityResources =>
            new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile()
            };

        public static IEnumerable<ApiScope> ApiScopes =>
            new ApiScope[]
            {
                new ApiScope("eshop-api", "eShop API")
            };

        public static IEnumerable<Client> Clients =>
            new Client[]
            {
                new Client
                {
                    ClientName = "eshop-api",
                    ClientId = "eshop-api",

                    // no interactive user, use the clientid/secret for authentication
                    AllowedGrantTypes = GrantTypes.ClientCredentials,

                    // secret for authentication
                    ClientSecrets =
                    {
                        new Secret("secret")
                    },

                    // scopes that client has access to
                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "eshop-api"
                    }
                },
                new Client
                {
                    ClientName = "eshop-ui",
                    ClientId = "eshop-ui",
                    ClientSecrets = { new Secret("secret") },

                    AllowedGrantTypes = GrantTypes.Code,
                    RequirePkce = true,
                    RequireClientSecret = false,

                    // where to redirect to after login
                    RedirectUris = { "https://localhost:5110/authentication/login-callback" },

                    // where to redirect to after logout
                    PostLogoutRedirectUris = { "https://localhost:5110/authentication/logout-callback" },

                    AllowedScopes = new List<string>
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "eshop-api"
                    }
                },
                new Client
                {
                    ClientName = "eshop-swagger",
                    ClientId = "eshop-swagger",
                    ClientSecrets = { new Secret("secret") },

                    AllowedGrantTypes = GrantTypes.Code,
                    RequirePkce = true,
                    RequireClientSecret = false,

                    // where to redirect to after login
                    RedirectUris = { "https://localhost:5100/swagger/oauth2-redirect.html" },

                    // where to redirect to after logout
                    PostLogoutRedirectUris = { "https://localhost:5101/authentication/logout-callback" },

                    AllowedScopes = new List<string>
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "eshop-api"
                    }
                }
            };
    }
}