using AutoMapper;
using Boleto.Contracts.Events.MovieEvents;
using Filter.API.Models;

namespace Filter.API.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<MovieCreated, Movie>().ReverseMap();
            CreateMap<MovieUpdated, Movie>().ReverseMap();
        }
    }
}
