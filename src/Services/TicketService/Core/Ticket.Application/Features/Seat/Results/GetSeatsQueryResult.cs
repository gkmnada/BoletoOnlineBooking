namespace Ticket.Application.Features.Seat.Results
{
    public class GetSeatsQueryResult
    {
        public string SeatID { get; set; }
        public string Row { get; set; }
        public int Number { get; set; }
        public string HallID { get; set; }
        public bool IsActive { get; set; }
    }
}
