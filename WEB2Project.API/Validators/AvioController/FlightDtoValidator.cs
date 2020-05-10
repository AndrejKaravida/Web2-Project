using FluentValidation;
using WEB2Project.Dtos;

namespace WEB2Project.Validators.AvioController
{
    class FlightDtoValidator : AbstractValidator<FlightDto>
    {
        public FlightDtoValidator()
        {
            RuleFor(x => x.StartingDestination).NotNull().MinimumLength(3).MaximumLength(50);
            RuleFor(x => x.ArrivalDestination).NotNull().MinimumLength(3).MaximumLength(50);
            RuleFor(x => x.ArrivalDate).NotNull().MinimumLength(3).MaximumLength(50);
            RuleFor(x => x.DepartureDate).NotNull().MinimumLength(3).MaximumLength(50);
        }
    }
}
