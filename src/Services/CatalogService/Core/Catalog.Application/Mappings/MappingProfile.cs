using AutoMapper;
using Boleto.Contracts.Events.MovieEvents;
using Catalog.Application.Features.Category.Commands;
using Catalog.Application.Features.Category.Results;
using Catalog.Application.Features.Movie.Commands;
using Catalog.Application.Features.Movie.Results;
using Catalog.Domain.Entities;

namespace Catalog.Application.Mappings
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

            // Movie mappings
            CreateMap<Movie, CreateMovieCommand>().ReverseMap();
            CreateMap<Movie, UpdateMovieCommand>().ReverseMap();
            CreateMap<Movie, GetMoviesQueryResult>().ReverseMap();
            CreateMap<Movie, GetMovieByIdQueryResult>().ReverseMap();

            CreateMap<Movie, MovieCreated>().ReverseMap();
            CreateMap<Movie, MovieUpdated>().ReverseMap();
            CreateMap<Movie, MovieDeleted>().ReverseMap();
        }
    }
}
