namespace Booking.API.Models
{
    public class MovieTicket
    {
        public string ticket_id { get; set; }
        public string session_id { get; set; }
        public string seat_id { get; set; }
        public string status { get; set; }
        public decimal price { get; set; }
        public DateTime created_at { get; set; }
        public string user_id { get; set; }
    }
}
