using AutoMapper;
using Boleto.Contracts.Events.TicketEvents;
using Payment.API.Features.Payment.Commands;
using Payment.API.Models;

namespace Payment.API.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Entities.Payment, CreatePaymentCommand>().ReverseMap();

            CreateMap<Ticket, TicketUpdated>().ReverseMap();
        }
    }
}
