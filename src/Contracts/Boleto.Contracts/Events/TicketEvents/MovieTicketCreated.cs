namespace Boleto.Contracts.Events.TicketEvents
{
    public class MovieTicketCreated
    {
        public string ticket_id { get; set; }
        public string session_id { get; set; }
        public string seat_id { get; set; }
        public string status { get; set; }
        public decimal price { get; set; }
        public DateTime created_at { get; set; }
    }
}
