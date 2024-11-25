using AutoMapper;
using Discount.API.Entities;
using Discount.API.Features.Coupon.Commands;
using Discount.API.Features.Coupon.Results;

namespace Discount.API.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Coupon, CreateCouponCommand>().ReverseMap();
            CreateMap<Coupon, UpdateCouponCommand>().ReverseMap();
            CreateMap<Coupon, GetCouponsQueryResult>().ReverseMap();
            CreateMap<Coupon, GetCouponByCodeQueryResult>().ReverseMap();
            CreateMap<Coupon, GetCouponByIdQueryResult>().ReverseMap();
        }
    }
}
