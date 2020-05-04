using FluentValidation;
using WEB2Project.Dtos;

namespace WEB2Project.Validators
{
    public class BranchToAddValidator : AbstractValidator<BranchToAdd>
    {
        public BranchToAddValidator()
        {
            RuleFor(x => x.Address).NotEmpty().MinimumLength(5).MaximumLength(50);
            RuleFor(x => x.City).NotEmpty().MinimumLength(2).MaximumLength(50);
            RuleFor(x => x.Country).NotEmpty().MinimumLength(2).MaximumLength(30);
        }
    }
}
