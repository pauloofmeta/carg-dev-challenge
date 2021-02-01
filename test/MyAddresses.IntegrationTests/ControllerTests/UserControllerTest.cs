using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using Xunit;

namespace MyAddresses.IntegrationTests.ControllerTests
{
    public class UserControllerTest: ControllerTestBase
    {
        public UserControllerTest(WebApplicationFactory<Webapi.Startup> factory)
            :base(factory)
        {
        }

        [Fact]
        public async Task WhenRegisteringUserSuccess()
        {
            var expected = new { username = "fake" };
            var result = await SendAsync(HttpMethod.Post, "/api/users", expected);
            result.Should().Be(expected);
        }

        [Fact]
        public async Task WhenRegisteringUserWithoutUsernameReturnError()
        {
            var expected = new { username = string.Empty };
            var result = await SendErrorAsync(HttpMethod.Post, "/api/users", expected);
            result.Errors.Should().NotBeEmpty();
        }

        
    }
}