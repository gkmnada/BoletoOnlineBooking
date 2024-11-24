namespace Boleto.Contracts.Events.TicketEvents
{
    public class MovieTicketUpdated
    {
        public string ticket_id { get; set; }
        public string status { get; set; }
        public decimal price { get; set; }
    }
}
