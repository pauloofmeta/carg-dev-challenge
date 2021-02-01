using FluentValidation;
using Microsoft.Extensions.Localization;
using MyAddresses.Domain;
using MyAddresses.Domain.Entities;

namespace MyAddresses.Services.Validators
{
    public class AddressValidator: AbstractValidator<Address>
    {
        public AddressValidator(IStringLocalizer<SharedResource> localizer)
        {
            RuleFor(r => r.Street).NotEmpty().WithMessage(localizer["Address.StreetNotBeEmpty"].Value);
            RuleFor(r => r.City).NotEmpty().WithMessage(localizer["Address.CityNotBeEmpty"].Value);
            RuleFor(r => r.State).NotEmpty().WithMessage(localizer["Address.StateNotBeEmpty"].Value);
            RuleFor(r => r.ZipCode).NotEmpty().WithMessage(localizer["Address.ZipCodeNotBeEmpty"].Value);
        }
    }
}