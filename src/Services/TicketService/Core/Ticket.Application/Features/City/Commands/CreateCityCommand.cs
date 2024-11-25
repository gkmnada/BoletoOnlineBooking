using MediatR;
using Ticket.Application.Common.Base;

namespace Ticket.Application.Features.City.Commands
{
    public class CreateCityCommand : IRequest<BaseResponse>
    {
        public string Name { get; set; }
    }
}
