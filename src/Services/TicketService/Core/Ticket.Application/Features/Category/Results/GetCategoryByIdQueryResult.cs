namespace Ticket.Application.Features.Category.Results
{
    public class GetCategoryByIdQueryResult
    {
        public string id { get; set; }
        public string name { get; set; }
        public bool is_active { get; set; }
    }
}
