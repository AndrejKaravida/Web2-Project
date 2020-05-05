using FluentValidation;
using WEB2Project.Dtos;

namespace WEB2Project.Validators.AuthController
{
    public class EmailDtoValidator : AbstractValidator<EmailDto>
    {
        public EmailDtoValidator()
        {
            RuleFor(x => x.Email).EmailAddress();
        }
    }
}
