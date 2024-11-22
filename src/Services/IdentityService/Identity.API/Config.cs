using Duende.IdentityServer;
using Duende.IdentityServer.Models;

namespace Identity.API
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
            },
            new ApiResource("TicketResource")
            {
                Scopes = { "TicketFullPermission", "TicketReadOnlyPermission" }
            },
            new ApiResource("DiscountResource")
            {
                Scopes = { "DiscountFullPermission", "DiscountReadOnlyPermission" }
            },
            new ApiResource("BookingResource")
            {
                Scopes = { "BookingFullPermission", "BookingReadOnlyPermission" }
            }
        };
        public static IEnumerable<ApiScope> ApiScopes => new ApiScope[]
        {
            new ApiScope("CatalogFullPermission", "Full access to catalog items"),
            new ApiScope("CatalogReadOnlyPermission", "Read only access to catalog items"),
            new ApiScope("TicketFullPermission", "Full access to ticket items"),
            new ApiScope("TicketReadOnlyPermission", "Read only access to ticket items"),
            new ApiScope("DiscountFullPermission", "Full access to discount items"),
            new ApiScope("DiscountReadOnlyPermission", "Read only access to discount items"),
            new ApiScope("BookingFullPermission", "Full access to booking items"),
            new ApiScope("BookingReadOnlyPermission", "Read only access to booking items")
        };
        public static IEnumerable<Client> Clients => new Client[]
        {
            new Client
            {
                ClientId = "VisitorClient",
                ClientName = "Visitor Client",
                AllowedGrantTypes = GrantTypes.ClientCredentials,
                ClientSecrets = { new Secret("DuendeVisitorClientSecret".Sha256()) },
                AllowedScopes = { "CatalogReadOnlyPermission", "TicketReadOnlyPermission", "DiscountReadOnlyPermission", "BookingReadOnlyPermission" }
            },
            new Client
            {
                ClientId = "AdminClient",
                ClientName = "Admin Client",
                AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                ClientSecrets = { new Secret("DuendeAdminClientSecret".Sha256()) },
                AllowedScopes = { "CatalogFullPermission", "TicketFullPermission", "DiscountFullPermission", "BookingFullPermission"
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Email,
                    IdentityServerConstants.StandardScopes.Profile
                }
            }
        };
    }
}
