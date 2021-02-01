using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using MyAddresses.Domain.Entities;
using MyAddresses.Domain.Models;
using MyAddresses.IntegrationTests.Fakers;
using MyAddresses.IntegrationTests.Mocks;
using MyAddresses.Services.ApiClients;
using MyAddresses.Webapi;
using Xunit;

namespace MyAddresses.IntegrationTests.ControllerTests
{
    public class AddressesControllerTest : ControllerTestBase
    {
        public AddressesControllerTest(WebApplicationFactory<Startup> factory)
            : base(factory)
        {
        }

        protected override void OnConfigure(IServiceCollection services) => 
            services.AddScoped(s => GoogleMapsApiMock.Get());

        [Fact]
        public async Task WhenCreatingAddressWithoutFieldsRequiredReturnError()
        {
            var result = await SendErrorAsync(HttpMethod.Post, "/api/addresses", new AddressModel());
            result.Errors.Should().NotBeEmpty();
        }

        [Fact]
        public async Task WhenCreatingAddressSuccess()
        {
            var expected = AddressModelFaker.Faker.Generate();
            await SendAsync(HttpMethod.Post, "/api/users", new User { Username = expected.Username });
            var result = await SendAsync(HttpMethod.Post, "/api/addresses", expected);
            result.Should().BeEquivalentTo(expected, o => o.Excluding(x => x.Username));
        }

        [Fact]
        public async Task WhenUpdatingAddressSuccess()
        {
            var expected = AddressModelFaker.Faker.Generate();
            await SendAsync(HttpMethod.Post, "/api/users",
                new User { Username = expected.Username });
            var address = await SendAsync<AddressModel, Address>(
                HttpMethod.Post, "/api/addresses", expected);
            expected.Complement = "fake";
            expected.Number = "0000";
            var result = await SendAsync(HttpMethod.Put,
                $"/api/addresses/{address.Id}", expected);
            result.Should().BeEquivalentTo(expected,
                o => o.Excluding(x => x.Username));
        }

        [Fact]
        public async Task WhenDeletingAddressSuccess()
        {
            var expected = AddressModelFaker.Faker.Generate();
            await SendAsync(HttpMethod.Post, "/api/users",
                new User { Username = expected.Username });
            var address = await SendAsync<AddressModel, Address>(
                HttpMethod.Post, "/api/addresses", expected);
            await SendAsync<AddressModel>(HttpMethod.Delete,
                $"/api/addresses/{address.Id}?username={expected.Username}");
            var result = await SendErrorAsync<Address>(HttpMethod.Get, $"/api/addresses/{address.Id}");
            result.Status.Should().Be(404);
        }

        [Fact]
        public async Task WhenGetAllAddressSuccess()
        {
            var expected = AddressModelFaker.Faker.Generate();
            await SendAsync(HttpMethod.Post, "/api/users",
                new User { Username = expected.Username });
            var address = await SendAsync<AddressModel, Address>(
                HttpMethod.Post, "/api/addresses", expected);
            var result  = await SendAsync<IEnumerable<Address>>(HttpMethod.Get,
                $"/api/addresses?username={expected.Username}");
            result.Should().NotBeEmpty();
            result.First().Should().BeEquivalentTo(address);
        }
    }
}