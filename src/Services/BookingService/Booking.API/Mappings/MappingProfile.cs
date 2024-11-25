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
            CreateMap<Ticket, TicketCreated>().ReverseMap();
            CreateMap<Ticket, TicketUpdated>().ReverseMap();
            CreateMap<Ticket, Checkout>().ReverseMap();

            CreateMap<Checkout, BookingCheckout>().ReverseMap();
        }
    }
}
