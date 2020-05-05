using FluentValidation;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WEB2Project.Validators
{
    public class IFormFileValidator : AbstractValidator<IFormFile>
    {
        public IFormFileValidator()
        {
            RuleFor(x => x.Length).NotNull().LessThanOrEqualTo(500);
        }
    }
}
