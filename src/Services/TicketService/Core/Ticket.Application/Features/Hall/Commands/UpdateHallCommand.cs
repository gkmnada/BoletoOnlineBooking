using MediatR;
using Ticket.Application.Common.Base;

namespace Ticket.Application.Features.Hall.Commands
{
    public class UpdateHallCommand : IRequest<BaseResponse>
    {
        public string id { get; set; }
        public string name { get; set; }
        public int capacity { get; set; }
        public string cinema_id { get; set; }
        public bool is_active { get; set; }
    }
}
