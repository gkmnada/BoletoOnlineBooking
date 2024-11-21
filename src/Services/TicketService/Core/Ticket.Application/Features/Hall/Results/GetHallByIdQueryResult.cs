namespace Ticket.Application.Features.Hall.Results
{
    public class GetHallByIdQueryResult
    {
        public string id { get; set; }
        public string name { get; set; }
        public int capacity { get; set; }
        public string cinema_id { get; set; }
        public bool is_active { get; set; }
    }
}
