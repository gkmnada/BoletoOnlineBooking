using AutoMapper;
using Boleto.Contracts.Events.MovieEvents;
using Search.API.Models;

namespace Search.API.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<MovieCreated, Movie>()
                .ForMember(x => x.ObjectID, options => options.Ignore()).ReverseMap();
            CreateMap<MovieUpdated, Movie>()
                .ForMember(x => x.ObjectID, options => options.Ignore()).ReverseMap();
        }
    }
}
