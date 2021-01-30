using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;
using MyAddresses.Domain;
using MyAddresses.Domain.Entities;
using MyAddresses.Domain.Enums;
using MyAddresses.Domain.Exceptions;
using MyAddresses.Domain.Models;
using MyAddresses.Repositories.Abstractions;
using MyAddresses.Services.Abstractions;
using MyAddresses.Services.ApiClients;
using MyAddresses.Services.Base;
using Refit;

namespace MyAddresses.Services.Services
{
    public class AddressesService : CrudServices<Address>, IAddressesService
    {
        private readonly IGoogleMapsApi _googleMapsApi;
        private readonly IConfiguration _configuration;
        private readonly ICrudService<User> _userService;
        private readonly IStringLocalizer<SharedResource> _localizer;

        public AddressesService(IRepository<Address> repository,
            IValidator<Address> validator,
            IGoogleMapsApi googleMapsApi,
            IConfiguration configuration,
            ICrudService<User> userService,
            IStringLocalizer<SharedResource> localizer)
            : base(repository, validator)
        {
            _googleMapsApi = googleMapsApi;
            _configuration = configuration;
            _userService = userService;
            _localizer = localizer;
        }

        public async Task<Address> AddByUserAsync(Address model, string username)
        {
            Validator.ValidateAndThrow(model);
            await ValidateAddressAsync(model);

            model.UserId = await HandleUserIdAsync(username);
            return await base.OnAddAsync(model);
        }

        public async Task<Address> UpdateByUserAsync(Address model, string username)
        {
            var address = await GetByUserAsync(model.Id, username);
            address.Complement = model.Complement;
            address.Number = model.Number;
            await Repository.UpdatePartialAsync(address, p => p.Complement, p => p.Number);
            return address;
        }

        private async Task<int> HandleUserIdAsync(string userName)
        {
            var user = await _userService.GetAllAsync(x => x.Username == userName);
            if (user == default || !user.Any())
                throw new AppValidationException(HttpStatusCode.BadRequest,
                        _localizer["Address.UserNotFound"].Value);
            return user.Single().Id;
        }

        private async Task ValidateAddressAsync(Address model)
        {
            try
            {
                var result = await _googleMapsApi.GeocodeAsync(new GoogleGeocodeQueryModel
                {
                    Key = _configuration.GetValue<string>("GOOGLE_MAPS_KEY"),
                    Address = FormattedAddress(model.Street, model.District, model.City, model.State)

                });
                if (result.Status != GoogleGeocodeStatus.OK)
                    throw new AppValidationException(HttpStatusCode.BadRequest,
                        _localizer["Address.AddressNotValid"].Value);

            } catch(ApiException e)
            {
                throw new AppValidationException(HttpStatusCode.BadRequest,
                    _localizer["Address.AddressNotValid"].Value,
                    e.Content);
            }
        }

        private string FormattedAddress(params string[] addressArgs) => string.Join(",", addressArgs);

        public Task<IEnumerable<Address>> GetAllByUser(string username) =>
            Repository.GetAllAsync(x => x.User.Username == username);

        public async Task DeleteByUserAsync(int id, string username)
        {
            var address = await GetByUserAsync(id, username);
            await Repository.DeleteAsync(address);
        }

        private async Task<Address> GetByUserAsync(int id, string username)
        {
            var address = await GetByIdAsync(id);
            if (address == default)
                throw new AppValidationException(HttpStatusCode.NotFound,
                    _localizer["Address.AddressNotFound"].Value);

            var userId = await HandleUserIdAsync(username);
            if (address.UserId != userId)
                throw new AppValidationException(HttpStatusCode.Forbidden,
                    _localizer["Address.AddresssDoesNotUser"].Value);

            return address;
        }
    }
}