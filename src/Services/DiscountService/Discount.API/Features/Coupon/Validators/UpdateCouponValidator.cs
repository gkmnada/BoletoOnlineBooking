using Discount.API.Features.Coupon.Commands;
using FluentValidation;

namespace Discount.API.Features.Coupon.Validators
{
    public class UpdateCouponValidator : AbstractValidator<UpdateCouponCommand>
    {
        public UpdateCouponValidator()
        {
            RuleFor(x => x.CouponCode).NotEmpty().WithMessage("Coupon code is required");
            RuleFor(x => x.Amount).NotEmpty().WithMessage("Amount is required")
                .GreaterThan(0).WithMessage("Amount must be greater than 0");
        }
    }
}
