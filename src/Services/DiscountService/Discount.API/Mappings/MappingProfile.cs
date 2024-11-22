using AutoMapper;
using Discount.API.Dtos.Coupon;
using Discount.API.Entities;

namespace Discount.API.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Coupon, CreateCouponDto>().ReverseMap();
            CreateMap<Coupon, UpdateCouponDto>().ReverseMap();
            CreateMap<Coupon, CouponDto>().ReverseMap();
            CreateMap<Coupon, ListCouponDto>().ReverseMap();
        }
    }
}
