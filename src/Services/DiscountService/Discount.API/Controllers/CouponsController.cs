using Discount.API.Features.Coupon.Commands;
using Discount.API.Features.Coupon.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Discount.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CouponsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CouponsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> ListCoupons()
        {
            var query = new GetCouponsQuery();
            var response = await _mediator.Send(query);
            return Ok(response);
        }

        [HttpGet("GetCoupon/{id}")]
        public async Task<IActionResult> GetCouponById(string id)
        {
            var query = new GetCouponByIdQuery(id);
            var response = await _mediator.Send(query);
            return Ok(response);
        }

        [HttpGet("GetCouponByCode/{code}")]
        public async Task<IActionResult> GetCouponByCode(string code)
        {
            var query = new GetCouponByCodeQuery(code);
            var response = await _mediator.Send(query);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCoupon(CreateCouponCommand createCouponCommand)
        {
            var response = await _mediator.Send(createCouponCommand);
            return Ok(response);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateCoupon(UpdateCouponCommand updateCouponCommand)
        {
            var response = await _mediator.Send(updateCouponCommand);
            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCoupon(string id)
        {
            var command = new DeleteCouponCommand(id);
            var response = await _mediator.Send(command);
            return Ok(response);
        }
    }
}
