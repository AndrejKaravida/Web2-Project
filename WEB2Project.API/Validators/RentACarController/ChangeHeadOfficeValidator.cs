using FluentValidation;
using WEB2Project.Dtos;

namespace WEB2Project.Validators
{
    public class ChangeHeadOfficeValidator : AbstractValidator<ChangeHeadOffice>
    {
        public ChangeHeadOfficeValidator()
        {
            RuleFor(x => x.HeadOffice).NotNull().MinimumLength(2).MaximumLength(50);
        }
    }
}
