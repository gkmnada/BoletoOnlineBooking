namespace Ticket.Application.Features.Cinema.Results
{
    public class GetCinemaByIdQueryResult
    {
        public string id { get; set; }
        public string name { get; set; }
        public string address { get; set; }
        public string city_id { get; set; }
        public bool is_active { get; set; }
    }
}
