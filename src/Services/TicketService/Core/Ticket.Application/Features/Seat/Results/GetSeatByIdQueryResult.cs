namespace Ticket.Application.Features.Seat.Results
{
    public class GetSeatByIdQueryResult
    {
        public string id { get; set; }
        public string row { get; set; }
        public int number { get; set; }
        public string hall_id { get; set; }
        public bool is_active { get; set; }
    }
}
