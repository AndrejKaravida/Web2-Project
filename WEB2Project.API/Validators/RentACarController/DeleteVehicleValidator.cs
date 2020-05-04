using FluentValidation;
using WEB2Project.Dtos;

namespace WEB2Project.Validators
{
    public class DeleteVehicleValidator : AbstractValidator<DeleteVehicle>
    {
        public DeleteVehicleValidator()
        {
            RuleFor(x => x.CompanyId).NotNull().GreaterThan(0);
        }
    }
}
