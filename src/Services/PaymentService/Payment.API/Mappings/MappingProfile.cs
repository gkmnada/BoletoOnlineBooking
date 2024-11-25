using AutoMapper;
using Boleto.Contracts.Events.TicketEvents;
using Payment.API.Models;

namespace Payment.API.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Ticket, TicketUpdated>().ReverseMap();
        }
    }
}
