using MediatR;
using Ticket.Application.Common.Base;

namespace Ticket.Application.Features.City.Commands
{
    public class DeleteCityCommand : IRequest<BaseResponse>
    {
        public string CityID { get; set; }

        public DeleteCityCommand(string id)
        {
            CityID = id;
        }
    }
}
