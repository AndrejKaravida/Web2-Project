using FluentValidation;
using WEB2Project.Dtos;

namespace WEB2Project.Validators.AuthController
{
    public class UserToUpdateValidator : AbstractValidator<UserToUpdate>
    {
        public UserToUpdateValidator()
        {
            RuleFor(x => x.email).EmailAddress();
            RuleFor(x => x.user_metadata).SetValidator(new UserMetadataValidator());
        }
    }
}
