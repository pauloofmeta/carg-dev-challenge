using Bogus;
using MyAddresses.Domain.Models;

namespace MyAddresses.IntegrationTests.Fakers
{
    public static class AddressModelFaker
    {
        public static Faker<AddressModel> Faker => new Faker<AddressModel>()
            .RuleFor(r => r.Street, f => f.Address.StreetName()) 
            .RuleFor(r => r.City, f => f.Address.City()) 
            .RuleFor(r => r.Complement, f => f.Address.Direction()) 
            .RuleFor(r => r.District, f => f.Address.SecondaryAddress()) 
            .RuleFor(r => r.Number, f => f.Address.BuildingNumber()) 
            .RuleFor(r => r.State, f => f.Address.State())
            .RuleFor(r => r.ZipCode, f => f.Address.ZipCode())
            .RuleFor(r => r.Username, f => f.Internet.UserName());
    }
}