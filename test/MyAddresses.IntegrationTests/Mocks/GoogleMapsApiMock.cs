using Moq;
using MyAddresses.Domain.Enums;
using MyAddresses.Domain.Models;
using MyAddresses.Services.ApiClients;

namespace MyAddresses.IntegrationTests.Mocks
{
    public static class GoogleMapsApiMock
    {
        public static IGoogleMapsApi Get()
        {
            var mock = new Mock<IGoogleMapsApi>();
            mock.Setup(m => m.GeocodeAsync(It.IsAny<GoogleGeocodeQueryModel>()))
                .ReturnsAsync(new GoogleGeocodeResultModel {Status = GoogleGeocodeStatus.OK});
            return mock.Object;
        }
    }
}