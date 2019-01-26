using Moq;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using Xvpn.BusinessLogic;

namespace Xvpn.Tests
{
    public class BusinessLogicTests
    {
        [Fact]
        private void XmlLocationResponseConverterTest()
        {
            string xvpnXmlResponse = File.ReadAllText("ExpressVpnResponse.xml");

            var converter = new XmlLocationResponseConverter();
            var response = converter.Convert(xvpnXmlResponse);

            Assert.NotNull(response);
            Assert.NotNull(response.Locations);
            Assert.Equal(2, response.Locations.Count());
        }

        [Fact]
        private void ButtonTextTest()
        {
            string xvpnXmlResponse = File.ReadAllText("ExpressVpnResponse.xml");
            var converter = new XmlLocationResponseConverter();
            var response = converter.Convert(xvpnXmlResponse);
            var location = response.Locations.ToArray()[0];

            Assert.NotNull(response.ButtonText);
            Assert.Equal("Place this text on the refresh button", response.ButtonText);
        }

        [Fact]
        private void Location0Test()
        {
            string xvpnXmlResponse = File.ReadAllText("ExpressVpnResponse.xml");
            var converter = new XmlLocationResponseConverter();
            var response = converter.Convert(xvpnXmlResponse);
            var location = response.Locations.ToArray()[0];

            Assert.NotNull(location);

            Assert.Equal("Los Angeles", location.Name);
            Assert.Equal(80, location.SortOrder);

            Assert.NotNull(location.Icon);
            Assert.Equal(IconData.IconId05Bytes, location.Icon.Value);

            Assert.NotNull(location.Servers);
            Assert.Equal(2, location.Servers.Count());

            var servers = location.Servers.ToArray();

            Assert.NotNull(servers[0]);
            Assert.NotNull(servers[0].IP);
            Assert.Equal(IPAddress.Parse("64.120.99.235") , servers[0].IP);

            Assert.NotNull(servers[1]);
            Assert.NotNull(servers[1].IP);
            Assert.Equal(IPAddress.Parse("173.234.147.130"), servers[1].IP);
        }

        [Fact]
        private void Location1Test()
        {
            string xvpnXmlResponse = File.ReadAllText("ExpressVpnResponse.xml");
            var converter = new XmlLocationResponseConverter();
            var response = converter.Convert(xvpnXmlResponse);
            var location = response.Locations.ToArray()[1];

            Assert.NotNull(location);

            Assert.Equal("UK - Isle of Man", location.Name);
            Assert.Equal(195, location.SortOrder);

            Assert.NotNull(location.Icon);
            Assert.Equal(IconData.IconId03Bytes, location.Icon.Value);

            Assert.NotNull(location.Servers);
            Assert.Single(location.Servers);

            var servers = location.Servers.ToArray();

            Assert.NotNull(servers[0]);
            Assert.NotNull(servers[0].IP);
            Assert.Equal(IPAddress.Parse("188.64.186.161"), servers[0].IP);
        }

        [Fact]
        private async void LocationServicePositiveTest()
        {
            string xvpnXmlResponse = File.ReadAllText("ExpressVpnResponse.xml");

            var response = new HttpResponseMessage();
            response.StatusCode = HttpStatusCode.OK;
            response.Content = new StringContent(xvpnXmlResponse); 

            var httpClientMock = new Mock<ICustomHttpClient>();
            httpClientMock.Setup(x => x.Execute(null, HttpVerb.Get, null)).Returns(Task.FromResult(response));

            var locationService = new LocationService(httpClientMock.Object, new XmlLocationResponseConverter());
            var xvpnLocations = await locationService.GetLocations();

            Assert.NotNull(xvpnLocations);
            Assert.Equal(2, xvpnLocations.Locations.Count());
        }

        [Fact]
        private async void LocationServiceNegativeTest()
        {
            var response = new HttpResponseMessage();
            response.StatusCode = HttpStatusCode.OK;

            var httpClientMock = new Mock<ICustomHttpClient>();
            httpClientMock.Setup(x => x.Execute(null, HttpVerb.Get, null)).Returns(Task.FromResult(response));

            var locationService = new LocationService(httpClientMock.Object, new XmlLocationResponseConverter());

            await Assert.ThrowsAsync<NullReferenceException>(() => locationService.GetLocations());
        }

        [Fact]
        private async void UnknownApiExceptionTest()
        {
            var httpClient = new CustomHttpClient();

            await Assert.ThrowsAsync<UnknownApiException>(
                () => httpClient.Execute(new Uri("http://alex.com"), HttpVerb.Get, null));

        }

    }
}