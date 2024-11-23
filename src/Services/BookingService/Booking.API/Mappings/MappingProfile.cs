using AutoMapper;
using Boleto.Contracts.Events.TicketEvents;
using Booking.API.Models;

namespace Booking.API.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<MovieTicket, Checkout>().ReverseMap();
            CreateMap<MovieTicket, MovieTicketCreated>().ReverseMap();
            CreateMap<MovieTicket, MovieTicketUpdated>().ReverseMap();
        }
    }
}
