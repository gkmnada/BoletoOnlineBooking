﻿using Booking.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Booking.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class BookingsController : ControllerBase
    {
        private readonly IBookingService _bookingService;

        public BookingsController(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }

        [HttpPut("ImplementCoupon")]
        public async Task<IActionResult> ImplementCoupon(string couponCode)
        {
            var response = await _bookingService.ImplementCouponAsync(couponCode);
            return Ok(response);
        }

        [HttpPost("BookingCheckout")]
        public async Task<IActionResult> BookingCheckout()
        {
            var response = await _bookingService.BookingCheckoutAsync();
            return Ok(response);
        }

        [HttpDelete("CancelCheckout")]
        public async Task<IActionResult> CancelCheckout()
        {
            var response = await _bookingService.CancelCheckoutAsync();
            return Ok(response);
        }
    }

}
