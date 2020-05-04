using FluentValidation;
using WEB2Project.Dtos;

namespace WEB2Project.Validators.AuthController
{
    public class CompanyAdminValidator : AbstractValidator<CompanyAdmin>
    {
        public CompanyAdminValidator()
        {
            RuleFor(x => x.CompanyId).NotNull().GreaterThan(0);
            RuleFor(x => x.Email).EmailAddress();
            RuleFor(x => x.FirstName).NotNull().MinimumLength(2).MaximumLength(50);
            RuleFor(x => x.LastName).NotNull().MinimumLength(2).MaximumLength(50);
            RuleFor(x => x.Password).NotNull().Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)[a-zA-Z\d]{8,}$");
        }
    }
}
