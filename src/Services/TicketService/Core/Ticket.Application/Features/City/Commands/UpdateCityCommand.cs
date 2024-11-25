using MediatR;
using Ticket.Application.Common.Base;

namespace Ticket.Application.Features.City.Commands
{
    public class UpdateCityCommand : IRequest<BaseResponse>
    {
        public string CityID { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
    }
}
