using AutoMapper;
using Boleto.Contracts.Events.BookingEvents;
using Boleto.Contracts.Events.TicketEvents;
using Booking.API.Models;

namespace Booking.API.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<MovieTicket, MovieTicketCreated>().ReverseMap();
            CreateMap<MovieTicket, MovieTicketUpdated>().ReverseMap();
            CreateMap<MovieTicket, Checkout>().ReverseMap();

            CreateMap<Checkout, BookingCheckout>().ReverseMap();
        }
    }
}
