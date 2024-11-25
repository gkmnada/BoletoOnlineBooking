using AutoMapper;
using Boleto.Contracts.Events.TicketEvents;
using Ticket.Application.Features.Category.Commands;
using Ticket.Application.Features.Category.Results;
using Ticket.Application.Features.Cinema.Commands;
using Ticket.Application.Features.Cinema.Results;
using Ticket.Application.Features.City.Commands;
using Ticket.Application.Features.City.Results;
using Ticket.Application.Features.Hall.Commands;
using Ticket.Application.Features.Hall.Results;
using Ticket.Application.Features.Ticket.Commands;
using Ticket.Application.Features.Pricing.Commands;
using Ticket.Application.Features.Pricing.Results;
using Ticket.Application.Features.Seat.Commands;
using Ticket.Application.Features.Seat.Results;
using Ticket.Application.Features.Session.Commands;
using Ticket.Application.Features.Session.Results;
using Ticket.Domain.Entities;

namespace Ticket.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Category mappings
            CreateMap<Category, CreateCategoryCommand>().ReverseMap();
            CreateMap<Category, UpdateCategoryCommand>().ReverseMap();
            CreateMap<Category, GetCategoriesQueryResult>().ReverseMap();
            CreateMap<Category, GetCategoryByIdQueryResult>().ReverseMap();

            // Cinema mappings
            CreateMap<Cinema, CreateCinemaCommand>().ReverseMap();
            CreateMap<Cinema, UpdateCinemaCommand>().ReverseMap();
            CreateMap<Cinema, GetCinemasQueryResult>().ReverseMap();
            CreateMap<Cinema, GetCinemaByIdQueryResult>().ReverseMap();

            // City mappings
            CreateMap<City, CreateCityCommand>().ReverseMap();
            CreateMap<City, UpdateCityCommand>().ReverseMap();
            CreateMap<City, GetCitiesQueryResult>().ReverseMap();
            CreateMap<City, GetCityByIdQueryResult>().ReverseMap();

            // Hall mappings
            CreateMap<Hall, CreateHallCommand>().ReverseMap();
            CreateMap<Hall, UpdateHallCommand>().ReverseMap();
            CreateMap<Hall, GetHallsQueryResult>().ReverseMap();
            CreateMap<Hall, GetHallByIdQueryResult>().ReverseMap();

            // Pricing mappings
            CreateMap<Pricing, CreatePricingCommand>().ReverseMap();
            CreateMap<Pricing, UpdatePricingCommand>().ReverseMap();
            CreateMap<Pricing, GetPricingsQueryResult>().ReverseMap();
            CreateMap<Pricing, GetPricingByIdQueryResult>().ReverseMap();

            // Seat mappings
            CreateMap<Seat, CreateSeatCommand>().ReverseMap();
            CreateMap<Seat, UpdateSeatCommand>().ReverseMap();
            CreateMap<Seat, GetSeatsQueryResult>().ReverseMap();
            CreateMap<Seat, GetSeatByIdQueryResult>().ReverseMap();

            // Session mappings
            CreateMap<Session, CreateSessionCommand>().ReverseMap();
            CreateMap<Session, UpdateSessionCommand>().ReverseMap();
            CreateMap<Session, GetSessionsQueryResult>().ReverseMap();
            CreateMap<Session, GetSessionByIdQueryResult>().ReverseMap();

            // Ticket mappings
            CreateMap<Domain.Entities.Ticket, CreateTicketCommand>().ReverseMap();

            CreateMap<Domain.Entities.Ticket, TicketCreated>()
                .ForMember(x => x.MovieID, options => options.MapFrom(z => z.Session.MovieID))
                .ForMember(x => x.CinemaID, options => options.MapFrom(z => z.Session.Cinema.CinemaID))
                .ForMember(x => x.HallID, options => options.MapFrom(z => z.Session.Hall.HallID)).ReverseMap();
            CreateMap<Domain.Entities.Ticket, TicketUpdated>().ReverseMap();
        }
    }
}
