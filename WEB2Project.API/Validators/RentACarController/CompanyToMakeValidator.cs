using FluentValidation;
using WEB2Project.Dtos;

namespace WEB2Project.Validators
{
    public class CompanyToMakeValidator : AbstractValidator<CompanyToMake>
    {
        public CompanyToMakeValidator()
        {
             RuleFor(x => x.Name).NotEmpty().MinimumLength(2).MaximumLength(30);
             RuleFor(x => x.City).NotEmpty().MinimumLength(2).MaximumLength(30);
             RuleFor(x => x.Address).NotEmpty().MinimumLength(5).MaximumLength(30);
             RuleFor(x => x.Country).NotEmpty().MinimumLength(2).MaximumLength(30);

        }
    }
}
