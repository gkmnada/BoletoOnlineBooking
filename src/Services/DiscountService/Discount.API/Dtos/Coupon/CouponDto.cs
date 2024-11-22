﻿namespace Discount.API.Dtos.Coupon
{
    public class CouponDto
    {
        public string CouponID { get; set; }
        public string CouponCode { get; set; }
        public int Amount { get; set; }
        public DateTime ExpirationDate { get; set; }
        public bool IsActive { get; set; }
    }
}
