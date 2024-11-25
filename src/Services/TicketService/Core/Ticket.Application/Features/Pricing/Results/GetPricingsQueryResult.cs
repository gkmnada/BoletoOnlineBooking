namespace Ticket.Application.Features.Pricing.Results
{
    public class GetPricingsQueryResult
    {
        public string PricingID { get; set; }
        public string SessionID { get; set; }
        public string CategoryID { get; set; }
        public decimal Price { get; set; }
        public bool IsActive { get; set; }
    }
}
