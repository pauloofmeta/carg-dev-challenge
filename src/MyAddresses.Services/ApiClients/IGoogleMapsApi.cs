using System.Threading.Tasks;
using MyAddresses.Domain.Models;
using Refit;

namespace MyAddresses.Services.ApiClients
{
    public interface IGoogleMapsApi
    {
        [Get("/geocode/json")]
        Task<GoogleGeocodeResultModel> GeocodeAsync(GoogleGeocodeQueryModel query);
    }
}