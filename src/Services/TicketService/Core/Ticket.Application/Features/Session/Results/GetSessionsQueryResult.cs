namespace Ticket.Application.Features.Session.Results
{
    public class GetSessionsQueryResult
    {
        public string SessionID { get; set; }
        public DateOnly SessionDate { get; set; }
        public TimeOnly SessionTime { get; set; }
        public string HallID { get; set; }
        public string CinemaID { get; set; }
        public string MovieID { get; set; }
        public bool IsActive { get; set; }
    }
}
