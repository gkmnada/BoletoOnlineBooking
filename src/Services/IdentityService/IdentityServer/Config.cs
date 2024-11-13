using Duende.IdentityServer;
using Duende.IdentityServer.Models;

namespace IdentityServer
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> IdentityResources => new IdentityResource[]
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Email(),
            new IdentityResources.Profile()
        };

        public static IEnumerable<ApiResource> ApiResources => new ApiResource[]
        {
            new ApiResource("CatalogResource")
            {
                Scopes = { "CatalogFullPermission", "CatalogReadOnlyPermission" }
            }
        };

        public static IEnumerable<ApiScope> ApiScopes => new ApiScope[]
        {
            new ApiScope("CatalogFullPermission", "Full access to catalog items"),
            new ApiScope("CatalogReadOnlyPermission", "Read only access to catalog items")
        };

        public static IEnumerable<Client> Clients => new Client[]
        {
            new Client
            {
                ClientId = "VisitorClient",
                ClientName = "Visitor Client",
                AllowedGrantTypes = GrantTypes.ClientCredentials,
                ClientSecrets = { new Secret("DuendeVisitorClientSecret".Sha256()) },
                AllowedScopes = { "CatalogReadOnlyPermission",
                    IdentityServerConstants.LocalApi.ScopeName
                }
            },
            new Client
            {
                ClientId = "AdminClient",
                ClientName = "Admin Client",
                AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                ClientSecrets = { new Secret("DuendeAdminClientSecret".Sha256()) },
                AllowedScopes = { "CatalogFullPermission",
                    IdentityServerConstants.LocalApi.ScopeName,
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Email,
                    IdentityServerConstants.StandardScopes.Profile
                }
            }
        };
    }
}
