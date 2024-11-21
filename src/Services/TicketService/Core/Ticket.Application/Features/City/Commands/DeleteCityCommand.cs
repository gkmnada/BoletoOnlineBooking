using MediatR;
using Ticket.Application.Common.Base;

namespace Ticket.Application.Features.City.Commands
{
    public class DeleteCityCommand : IRequest<BaseResponse>
    {
        public string city_id { get; set; }

        public DeleteCityCommand(string id)
        {
            city_id = id;
        }
    }
}
