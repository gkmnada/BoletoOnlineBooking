using AutoMapper;
using Ticket.Application.Features.Category.Commands;
using Ticket.Application.Features.Category.Results;
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
        }
    }
}
