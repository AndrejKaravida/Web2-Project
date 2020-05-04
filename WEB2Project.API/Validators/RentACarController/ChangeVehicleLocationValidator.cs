using FluentValidation;
using WEB2Project.Dtos;

namespace WEB2Project.Validators
{
    public class ChangeVehicleLocationValidator : AbstractValidator<ChangeVehicleLocation>
    {
        public ChangeVehicleLocationValidator()
        {
            RuleFor(x => x.CompanyId).NotNull().GreaterThan(0);
            RuleFor(x => x.NewCity).NotNull().MinimumLength(2).MaximumLength(50);
        }
    }
}
