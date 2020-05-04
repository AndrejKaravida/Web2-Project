using FluentValidation;
using WEB2Project.Dtos;

namespace WEB2Project.Validators.AuthController
{
    public class UpdatePasswordValidator : AbstractValidator<UpdatePassword>
    {
        public UpdatePasswordValidator()
        {
            RuleFor(x => x.email).EmailAddress();

            //Minimum eight characters, at least one uppercase letter, one lowercase letter and one number:
            RuleFor(x => x.password).Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)[a-zA-Z\d]{8,}$"); 
        }
    }
}
