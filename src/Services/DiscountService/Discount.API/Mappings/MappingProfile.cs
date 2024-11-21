using AutoMapper;
using Discount.API.Entities;
using Discount.API.Models;

namespace Discount.API.Mappings
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            CreateMap<Coupon, CouponModel>().ReverseMap();
        }
    }
}
