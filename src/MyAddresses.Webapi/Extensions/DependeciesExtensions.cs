using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using MyAddresses.Repositories.Abstractions;
using MyAddresses.Services.Abstractions;
using MyAddresses.Services.Base;
using MyAddresses.Repositories.Repositories;
using MyAddresses.Services.Validators;
using MyAddresses.Domain.Entities;
using FluentValidation;
using Refit;
using MyAddresses.Services.ApiClients;
using System;
using MyAddresses.Services.Services;

namespace MyAddresses.Webapi.Extensions
{

    public static class DependeciesExtensions
    {
        private const string GoogleMapsApiUrl = "https://maps.googleapis.com/maps/api";

        public static void AddDepencies(this IServiceCollection services)
        {
            services.TryAddScoped(typeof(ICrudService<>), typeof(CrudServices<>));
            services.TryAddScoped(typeof(IRepository<>), typeof(BaseRepository<>));
            services.TryAddScoped<IValidator<User>, UserValidator>();
            services.TryAddScoped<IValidator<Address>, AddressValidator>();
            services.TryAddScoped<IAddressesService, AddressesService>();
            services.AddRefitClient<IGoogleMapsApi>()
                .ConfigureHttpClient(c => c.BaseAddress = new Uri(GoogleMapsApiUrl));
        }
    }
}