using FluentValidation;
using FreeCourse.Web.Models.Course;

namespace FreeCourse.Web.Validators.Course
{
    public class CourseUpdateInputValidator:AbstractValidator<CourseUpdateInput>
    {
        public CourseUpdateInputValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("İsim Alanı Boş Olamaz");
            RuleFor(x => x.Description).NotEmpty().WithMessage("Açıklama Alanı Boş Olamaz");
            RuleFor(x => x.Feature.Duration).InclusiveBetween(1, int.MaxValue).WithMessage("Süre Alanı Boş Olamaz");
            // 1423,54
            RuleFor(x => x.Price).NotEmpty().WithMessage("Fiyat Alanı Boş Olamaz").ScalePrecision(2, 6).WithMessage("Hatalı Para Formatı");
        }
    }
}
