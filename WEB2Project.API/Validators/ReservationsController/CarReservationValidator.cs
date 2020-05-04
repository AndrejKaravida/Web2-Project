using FluentValidation;
using WEB2Project.Dtos;

namespace WEB2Project.Validators.ReservationsController
{
    public class CarReservationValidator : AbstractValidator<CarReservtion>
    {
        public CarReservationValidator()
        {
            RuleFor(x => x.VehicleId).NotNull().GreaterThan(0);
            RuleFor(x => x.CompanyId).NotNull().GreaterThan(0);
            RuleFor(x => x.Startdate).NotNull().MinimumLength(5).MaximumLength(20);
            RuleFor(x => x.Enddate).NotNull().MinimumLength(5).MaximumLength(20);
            RuleFor(x => x.Username).NotNull().MinimumLength(2).MaximumLength(50);
            RuleFor(x => x.Companyname).NotNull().MinimumLength(2).MaximumLength(50);
            RuleFor(x => x.StartingLocation).NotNull().MinimumLength(2).MaximumLength(50);
            RuleFor(x => x.ReturningLocation).NotNull().MinimumLength(2).MaximumLength(50);
            RuleFor(x => x.Totaldays).NotNull().GreaterThan(0).LessThan(50);
            RuleFor(x => x.Totalprice).NotNull().GreaterThan(0).LessThan(100000);
        }
    }
}
