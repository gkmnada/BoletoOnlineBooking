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
            },
            new ApiResource("PaymentResource")
            {
                Scopes = { "PaymentFullPermission", "PaymentReadOnlyPermission" }
            },
            new ApiResource("OrderResource")
            {
                Scopes = { "OrderFullPermission", "OrderReadOnlyPermission" }
            },
            new ApiResource("GatewayResource")
            {
                Scopes = { "CatalogFullPermission", "TicketFullPermission", "DiscountFullPermission", "BookingFullPermission", "PaymentFullPermission", "OrderFullPermission" }
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
            new ApiScope("BookingReadOnlyPermission", "Read only access to booking items"),
            new ApiScope("PaymentFullPermission", "Full access to payment items"),
            new ApiScope("PaymentReadOnlyPermission", "Read only access to payment items"),
            new ApiScope("OrderFullPermission", "Full access to order items"),
            new ApiScope("OrderReadOnlyPermission", "Read only access to order items"),
        };
        public static IEnumerable<Client> Clients => new Client[]
        {
            new Client
            {
                ClientId = "VisitorClient",
                ClientName = "Visitor Client",
                AllowedGrantTypes = GrantTypes.ClientCredentials,
                ClientSecrets = { new Secret("DuendeVisitorClientSecret".Sha256()) },
                AllowedScopes = { "CatalogReadOnlyPermission", "TicketReadOnlyPermission" }
            },
            new Client
            {
                ClientId = "AdminClient",
                ClientName = "Admin Client",
                AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                ClientSecrets = { new Secret("DuendeAdminClientSecret".Sha256()) },
                AllowedScopes = { "CatalogFullPermission", "TicketFullPermission", "DiscountFullPermission", "BookingFullPermission", "PaymentFullPermission", "OrderFullPermission",
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Email,
                    IdentityServerConstants.StandardScopes.Profile
                }
            }
        };
    }
}
