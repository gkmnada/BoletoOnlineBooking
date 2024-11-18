using AutoMapper;
using Boleto.Contracts.Events.MovieEvents;
using Catalog.Application.Features.Category.Commands;
using Catalog.Application.Features.Category.Results;
using Catalog.Application.Features.Movie.Commands;
using Catalog.Application.Features.Movie.Results;
using Catalog.Application.Features.MovieDetail.Commands;
using Catalog.Application.Features.MovieDetail.Results;
using Catalog.Application.Features.MovieImage.Commands;
using Catalog.Application.Features.MovieImage.Results;
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

            // MovieImage mappings
            CreateMap<MovieImage, CreateMovieImageCommand>().ReverseMap();
            CreateMap<MovieImage, UpdateMovieImageCommand>().ReverseMap();
            CreateMap<MovieImage, GetMovieImagesQueryResult>().ReverseMap();
            CreateMap<MovieImage, GetMovieImageByIdQueryResult>().ReverseMap();

            // MovieDetail mappings
            CreateMap<MovieDetail, CreateMovieDetailCommand>().ReverseMap();
            CreateMap<MovieDetail, UpdateMovieDetailCommand>().ReverseMap();
            CreateMap<MovieDetail, GetMovieDetailByMovieIdQueryResult>().ReverseMap();
            CreateMap<MovieDetail, GetMovieDetailByIdQueryResult>().ReverseMap();
        }
    }
}
