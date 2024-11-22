using AutoMapper;
using Discount.API.Common.Base;
using Discount.API.Context;
using Discount.API.Dtos.Coupon;
using Discount.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace Discount.API.Repositories
{
    public class CouponRepository : ICouponRepository
    {
        private readonly ApplicationContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<CouponRepository> _logger;

        public CouponRepository(ApplicationContext context, IMapper mapper, ILogger<CouponRepository> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<BaseResponse> CreateCouponAsync(CreateCouponDto createCouponDto)
        {
            try
            {
                var entity = _mapper.Map<Coupon>(createCouponDto);

                await _context.Coupons.AddAsync(entity);
                await _context.SaveChangesAsync();

                return new BaseResponse
                {
                    IsSuccess = true,
                    Message = "Coupon created successfully",
                    Data = new
                    {
                        CouponID = entity.CouponID
                    }
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating the coupon");
                throw new Exception("An error occurred while processing the request", ex);
            }
        }

        public async Task<BaseResponse> DeleteCouponAsync(string id)
        {
            try
            {
                var values = await _context.Coupons.FirstOrDefaultAsync(x => x.CouponID == id);

                if (values == null)
                {
                    return new BaseResponse
                    {
                        IsSuccess = false,
                        Message = "Coupon not found"
                    };
                }

                _context.Coupons.Remove(values);
                await _context.SaveChangesAsync();

                return new BaseResponse
                {
                    IsSuccess = true,
                    Message = "Coupon deleted successfully",
                    Data = new
                    {
                        CouponID = values.CouponID
                    }
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting the coupon");
                throw new Exception("An error occurred while processing the request", ex);
            }
        }

        public async Task<CouponDto> GetCouponByCodeAsync(string code)
        {
            try
            {
                var values = await _context.Coupons.FirstOrDefaultAsync(x => x.CouponCode == code);
                return _mapper.Map<CouponDto>(values);
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred while fetching the coupon by code");
                throw new Exception("An error occurred while processing the request", ex);
            }
        }

        public async Task<CouponDto> GetCouponByIdAsync(string id)
        {
            try
            {
                var values = await _context.Coupons.FirstOrDefaultAsync(x => x.CouponID == id);
                return _mapper.Map<CouponDto>(values);
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred while fetching the coupon by id");
                throw new Exception("An error occurred while processing the request", ex);
            }
        }

        public async Task<List<ListCouponDto>> ListCouponAsync()
        {
            try
            {
                var values = await _context.Coupons.ToListAsync();
                return _mapper.Map<List<ListCouponDto>>(values);
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred while fetching the coupons");
                throw new Exception("An error occurred while processing the request", ex);
            }
        }

        public async Task<BaseResponse> UpdateCouponAsync(UpdateCouponDto updateCouponDto)
        {
            try
            {
                var values = await _context.Coupons.FirstOrDefaultAsync(x => x.CouponID == updateCouponDto.CouponID);

                if (values == null)
                {
                    return new BaseResponse
                    {
                        IsSuccess = false,
                        Message = "Coupon not found"
                    };
                }

                _mapper.Map(updateCouponDto, values);

                _context.Update(values);
                await _context.SaveChangesAsync();

                return new BaseResponse
                {
                    IsSuccess = true,
                    Message = "Coupon updated successfully",
                    Data = new
                    {
                        CouponID = values.CouponID
                    }
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating the coupon");
                throw new Exception("An error occurred while processing the request", ex);
            }
        }
    }
}
