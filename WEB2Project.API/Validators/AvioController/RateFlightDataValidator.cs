using FluentValidation;
using WEB2Project.Dtos;

namespace WEB2Project.Validators.AvioController
{
    public class RateFlightDataValidator : AbstractValidator<RateFlightData>
    {
        public RateFlightDataValidator()
        {
            RuleFor(x => x.UserId).NotNull().MinimumLength(1).MaximumLength(50);
            RuleFor(x => x.FlightId).NotNull().GreaterThan(0);
            RuleFor(x => x.CompanyId).NotNull().GreaterThan(0);
            RuleFor(x => x.FlightRating).NotNull();
            RuleFor(x => x.CompanyRating).NotNull();
        }
    }
}
