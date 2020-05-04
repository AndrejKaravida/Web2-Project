using FluentValidation;
using WEB2Project.Dtos;

namespace WEB2Project.Validators
{
    public class VehicleToMakeValidator : AbstractValidator<VehicleToMake>
    {
        public VehicleToMakeValidator()
        {
            RuleFor(x => x.Manufacturer).NotNull().MinimumLength(2).MaximumLength(20);
            RuleFor(x => x.Model).NotNull().MinimumLength(2).MaximumLength(20);
            RuleFor(x => x.CurrentDestination).NotNull().MinimumLength(2).MaximumLength(20);
            RuleFor(x => x.Doors).NotNull().GreaterThan(0).LessThan(8);
            RuleFor(x => x.Seats).NotNull().GreaterThan(0).LessThan(8);
            RuleFor(x => x.Price).NotNull().MinimumLength(1).MaximumLength(5);
            RuleFor(x => x.Type).NotNull().MinimumLength(2).MaximumLength(20);
        }
    }
}
