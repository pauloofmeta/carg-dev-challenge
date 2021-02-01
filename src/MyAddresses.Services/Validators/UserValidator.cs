using FluentValidation;
using Microsoft.Extensions.Localization;
using MyAddresses.Domain.Entities;
using MyAddresses.Domain;

namespace MyAddresses.Services.Validators
{
    public class UserValidator: AbstractValidator<User>
    {
        public UserValidator(IStringLocalizer<SharedResource> localizer)
        {
            RuleFor(r => r.Username).NotEmpty().WithMessage(localizer["User.UserNameNotBeNull"].Value);
        }
    }
}