using FluentValidation;
using WEB2Project.Dtos;

namespace WEB2Project.Validators
{
    public class IncomeDataValidator : AbstractValidator<IncomeData>
    {
        public IncomeDataValidator()
        {
            RuleFor(x => x.StartingDate).NotNull();
            RuleFor(x => x.FinalDate).NotNull();
        }
    }
}
