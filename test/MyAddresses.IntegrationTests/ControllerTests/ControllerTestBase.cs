using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MyAddresses.Repositories.Abstractions;
using Newtonsoft.Json;
using Xunit;

namespace MyAddresses.IntegrationTests.ControllerTests
{
    public abstract class ControllerTestBase: IClassFixture<WebApplicationFactory<Webapi.Startup>>
    {
        private readonly WebApplicationFactory<Webapi.Startup> _factory;
        private readonly HttpClient _client;

        public ControllerTestBase(WebApplicationFactory<Webapi.Startup> factory)
        {
            _factory = factory;
            _client = _factory
            .WithWebHostBuilder(b => b.ConfigureServices(OnConfigure))
            .CreateClient();
        }

        protected async Task<T> SendAsync<T>(HttpMethod method, string route, bool ensureError = true)
        {
            var content = await RequestAsync<T>(method, route, ensureError);
            return string.IsNullOrEmpty(content) 
                ? default
                : JsonConvert.DeserializeObject<T>(content);
        }

        protected async Task<T> SendAsync<T>(HttpMethod method, string route, T model)
        {
            var content = await RequestAsync(method, route, model, true);
            return string.IsNullOrEmpty(content) 
                ? default
                : JsonConvert.DeserializeObject<T>(content);
        }

        protected async Task<TResult> SendAsync<T, TResult>(HttpMethod method, string route, T model)
        {
            var content = await RequestAsync(method, route, model, true);
            return string.IsNullOrEmpty(content) 
                ? default
                : JsonConvert.DeserializeObject<TResult>(content);
        }

        protected async Task<Webapi.Extensions.ValidationProblemDetails> SendErrorAsync<T>(
            HttpMethod method, string route, T model)
        {
            var content = await RequestAsync(method, route, model, false);
            return string.IsNullOrEmpty(content)
                ? default
                : JsonConvert.DeserializeObject<Webapi.Extensions.ValidationProblemDetails>(content);
        }

        protected async Task<Webapi.Extensions.ValidationProblemDetails> SendErrorAsync<T>(
            HttpMethod method, string route)
        {
            var content = await RequestAsync<T>(method, route, false);
            return string.IsNullOrEmpty(content)
                ? default
                : JsonConvert.DeserializeObject<Webapi.Extensions.ValidationProblemDetails>(content);
        }

        private async Task<string> RequestAsync<T>(HttpMethod method, string route, T data, bool ensureError) 
        {
            var json = JsonConvert.SerializeObject(data);
            var request = new HttpRequestMessage(method, route);
            request.Content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _client.SendAsync(request);

            if (ensureError)
                response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        private async Task<string> RequestAsync<T>(HttpMethod method, string route, bool ensureError) 
        {
            var request = new HttpRequestMessage(method, route);
            var response = await _client.SendAsync(request);

            if (ensureError)
                response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        protected virtual void OnConfigure(IServiceCollection services)
        {
        }
    }
}