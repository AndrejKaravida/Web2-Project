using FluentValidation;
using WEB2Project.Dtos;

namespace WEB2Project.Validators
{
    public class CompanyToEditValidator : AbstractValidator<CompanyToEdit>
    {
        public CompanyToEditValidator()
        {
            RuleFor(x => x.Name).NotNull().MinimumLength(2).MaximumLength(20);
            RuleFor(x => x.PromoDescription).NotNull().MinimumLength(2).MaximumLength(20);
            RuleFor(x => x.WeekRentalDiscount).GreaterThan(0).LessThan(100);
            RuleFor(x => x.MonthRentalDiscount).GreaterThan(0).LessThan(100);
        }
    }
}
