namespace Ticket.Application.Features.Cinema.Results
{
    public class GetCinemaByIdQueryResult
    {
        public string CinemaID { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string CityID { get; set; }
        public bool IsActive { get; set; }
    }
}
