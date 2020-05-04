using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WEB2Project.Dtos;

namespace WEB2Project.Validators.AuthController
{
    public class UserMetadataValidator : AbstractValidator<UserMetadata>
    {
        public UserMetadataValidator()
        {
            RuleFor(x => x.first_name).NotNull().MinimumLength(2).MaximumLength(30);
            RuleFor(x => x.last_name).NotNull().MinimumLength(2).MaximumLength(30);
            RuleFor(x => x.city).NotNull().MinimumLength(2).MaximumLength(30);
            RuleFor(x => x.phone_number).NotNull().MinimumLength(2).MaximumLength(30);
        }
    }
}
