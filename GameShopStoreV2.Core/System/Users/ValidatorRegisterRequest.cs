using FluentValidation;
using Microsoft.AspNetCore.WebUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameShopStoreV2.Core.System.Users
{
    public class ValidatorRegisterRequest : AbstractValidator<RegisterRequest>
    {
        public ValidatorRegisterRequest()
        {
            RuleFor(x => x.UserName).NotEmpty().WithMessage("User name is required");
            RuleFor(x => x.Password).NotEmpty().WithMessage("Password is required")
                .MinimumLength(8).WithMessage("Password must have at least 8 characters");

            RuleFor(x => x).Custom((request, context) =>
            {
                if (request.Password != request.ConfirmPassword)
                {
                    context.AddFailure("Confirm password doesn't match");
                }
            });
        }
    }
}
