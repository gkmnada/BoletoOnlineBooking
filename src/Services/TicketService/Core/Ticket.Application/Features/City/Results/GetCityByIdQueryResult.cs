namespace Ticket.Application.Features.City.Results
{
    public class GetCityByIdQueryResult
    {
        public string CityID { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
    }
}
