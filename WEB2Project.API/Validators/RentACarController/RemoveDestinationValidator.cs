using FluentValidation;
using WEB2Project.Dtos;

namespace WEB2Project.Validators
{
    public class RemoveDestinationValidator : AbstractValidator<RemoveDestination>
    {
        public RemoveDestinationValidator()
        {
            RuleFor(x => x.Location).NotNull().MinimumLength(2).MaximumLength(50);
        }
    }
}
