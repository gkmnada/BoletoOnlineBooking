namespace Ticket.Application.Features.Hall.Results
{
    public class GetHallByIdQueryResult
    {
        public string HallID { get; set; }
        public string Name { get; set; }
        public int Capacity { get; set; }
        public string CinemaID { get; set; }
        public bool IsActive { get; set; }
    }
}
