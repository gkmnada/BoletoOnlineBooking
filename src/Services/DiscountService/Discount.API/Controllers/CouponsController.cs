using Discount.API.Models;
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

        [HttpPost]
        public async Task<IActionResult> CreateCoupon(CouponModel couponModel)
        {
            try
            {
                await _couponRepository.CreateCouponAsync(couponModel);
                return Ok("Created Coupon");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating the coupon");
                throw new Exception("An error occurred while processing the request", ex);
            }
        }
    }
}
