using Discount.API.Dtos.Coupon;
using Discount.API.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Discount.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CouponsController : ControllerBase
    {
        private readonly ICouponRepository _couponRepository;
        private readonly ILogger<CouponsController> _logger;

        public CouponsController(ICouponRepository couponRepository, ILogger<CouponsController> logger)
        {
            _couponRepository = couponRepository;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> ListCoupons()
        {
            var response = await _couponRepository.ListCouponAsync();
            return Ok(response);
        }

        [HttpGet("GetCoupon/{id}")]
        public async Task<IActionResult> GetCouponById(string id)
        {
            var response = await _couponRepository.GetCouponByIdAsync(id);
            return Ok(response);
        }

        [HttpGet("GetCouponByCode/{code}")]
        public async Task<IActionResult> GetCouponByCode(string code)
        {
            var response = await _couponRepository.GetCouponByCodeAsync(code);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCoupon(CreateCouponDto createCouponDto)
        {
            var response = await _couponRepository.CreateCouponAsync(createCouponDto);
            return Ok(response);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateCoupon(UpdateCouponDto updateCouponDto)
        {
            var response = await _couponRepository.UpdateCouponAsync(updateCouponDto);
            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCoupon(string id)
        {
            var response = await _couponRepository.DeleteCouponAsync(id);
            return Ok(response);
        }
    }
}
