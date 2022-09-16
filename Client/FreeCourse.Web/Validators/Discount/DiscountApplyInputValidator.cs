using FluentValidation;
using FreeCourse.Web.Models.Discount;

namespace FreeCourse.Web.Validators.Discount
{
    public class DiscountApplyInputValidator : AbstractValidator<DiscountApplyInput>
    {
        public DiscountApplyInputValidator()
        {

            RuleFor(x => x.Code).NotEmpty().WithMessage("İndirim kodu alanı boş geçilemez");
        }
    }
}
