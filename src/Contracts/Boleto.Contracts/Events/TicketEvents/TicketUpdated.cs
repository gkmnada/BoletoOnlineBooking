namespace Boleto.Contracts.Events.TicketEvents
{
    public class TicketUpdated
    {
        public string TicketID { get; set; }
        public string Status { get; set; }
        public decimal Price { get; set; }
    }
}
