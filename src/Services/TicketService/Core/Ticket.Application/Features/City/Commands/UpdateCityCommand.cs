using MediatR;
using Ticket.Application.Common.Base;

namespace Ticket.Application.Features.City.Commands
{
    public class UpdateCityCommand : IRequest<BaseResponse>
    {
        public string id { get; set; }
        public string name { get; set; }
        public bool is_active { get; set; }
    }
}
