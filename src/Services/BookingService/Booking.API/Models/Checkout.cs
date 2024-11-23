namespace Booking.API.Models
{
    public class Checkout
    {
        public string ticket_id { get; set; }
        public string movie_id { get; set; }
        public string cinema_id { get; set; }
        public string hall_id { get; set; }
        public string session_id { get; set; }
        public string seat_id { get; set; }
        public string status { get; set; }
        public decimal price { get; set; }
        public string user_id { get; set; }
    }
}
