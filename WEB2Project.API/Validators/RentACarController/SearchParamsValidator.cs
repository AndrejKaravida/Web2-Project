using FluentValidation;
using WEB2Project.Dtos;

namespace WEB2Project.Validators.RentACarController
{
    public class SearchParamsValidator : AbstractValidator<SearchParams>
    {
        public SearchParamsValidator()
        {
            RuleFor(x => x.Location).NotNull().MinimumLength(2).MaximumLength(50);
            RuleFor(x => x.StartingDate).NotNull().MinimumLength(2).MaximumLength(20);
            RuleFor(x => x.ReturningDate).NotNull().MinimumLength(2).MaximumLength(20);
        }
    }
}
