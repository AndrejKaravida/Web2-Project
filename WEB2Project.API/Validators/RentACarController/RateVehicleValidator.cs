using FluentValidation;
using WEB2Project.Dtos;

namespace WEB2Project.Validators
{
    public class RateVehicleValidator : AbstractValidator<RateVehicle>
    {
        public RateVehicleValidator()
        {
            RuleFor(x => x.Rating).NotNull().GreaterThan(4).LessThan(11);
        }
    }
}
