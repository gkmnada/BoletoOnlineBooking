using AutoMapper;
using Order.Application.Features.Order.Commands;

namespace Order.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CreateOrderWithDetailCommand, Domain.Entities.Order>().ReverseMap();
            CreateMap<CreateOrderDetailCommand, Domain.Entities.OrderDetail>().ReverseMap();
        }
    }
}
