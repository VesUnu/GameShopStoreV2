using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameShopStoreV2.Core.System.Users
{
    public class ValidatorLoginRequest : AbstractValidator<LoginRequest>
    {
        public ValidatorLoginRequest()
        {
            RuleFor(x => x.UserName).NotEmpty().WithMessage("Username is required!");
            RuleFor(x => x.Password).NotEmpty().WithMessage("Passowrd is required!")
                .MinimumLength(8).WithMessage("Password must have at least 8 characters");
        }
    }
}
