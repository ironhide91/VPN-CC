using Moq;
using Prism.Interactivity.InteractionRequest;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using Xvpn.BusinessLogic;
using Xvpn.BusinessLogic.DataModels;
using Xvpn.UI.Helper;
using Xvpn.UI.LocationModule;

namespace Xvpn.Tests
{
    public class UITests
    {
        [Fact]
        private void LocationServiceNullTest()
        {
            Assert.Throws<ArgumentNullException>(() => new LocationViewModel(null, null));
        }

        [Fact]
        private void PingerFactoryNullTest()
        {
            var serviceMock = new Mock<ILocationService>();

            Assert.Throws<ArgumentNullException>(() => new LocationViewModel(serviceMock.Object, null));
        }

        [Fact]
        private void ButonTextTest()
        {
            var xvpnLocations = new XvpnLocations();
            xvpnLocations.ButtonText = "Test Button Text";

            var converterMock = new Mock<ILocationResponseConverter>();
            converterMock.Setup(x => x.Convert(null)).Returns(xvpnLocations);
            var typedLocations = converterMock.Object.Convert(null);

            var serviceMock = new Mock<ILocationService>();
            serviceMock.Setup(x => x.GetLocations()).Returns(Task.FromResult(typedLocations));

            var pingerMock = new Mock<IExecutePing>();
            pingerMock.Setup(x => x.RoundtripTime).Returns(1L);

            var pingerFactoryMock = new Mock<IExecutePingFactory>();
            pingerFactoryMock.Setup(x => x.Create()).Returns(pingerMock.Object);

            var viewModel = new LocationViewModel(serviceMock.Object, pingerFactoryMock.Object);

            viewModel.RefreshCommand.Execute(null);

            Assert.NotNull(viewModel.ButtonText);
            Assert.Equal(xvpnLocations.ButtonText, viewModel.ButtonText);
        }

        [Fact]
        private void LocationsTest()
        {
            var location0 = new Location();
            location0.Name = "India - Mumbai";
            location0.SortOrder = 99;
            location0.Icon = new Icon() { Id = 1, Value = IconData.IconId03Bytes };
            location0.Servers = new Server[]
            {
                new Server() { IP = IPAddress.Parse("10.10.10.10") },
                new Server() { IP = IPAddress.Parse("20.20.20.20") }
            };

            var location1 = new Location();
            location1.Name = "India - Banglore";
            location1.SortOrder = 99;
            location1.Icon = new Icon() { Id = 1, Value = IconData.IconId03Bytes };
            location1.Servers = new Server[]
            {
                new Server() { IP = IPAddress.Parse("30.30.30.30") }
            };

            var xvpnLocations = new XvpnLocations();
            xvpnLocations.ButtonText = "Test Button Text";
            xvpnLocations.Locations = new Location[] { location0, location1 };

            var converterMock = new Mock<ILocationResponseConverter>();
            converterMock.Setup(x => x.Convert(null)).Returns(xvpnLocations);
            var typedLocations = converterMock.Object.Convert(null);

            var serviceMock = new Mock<ILocationService>();
            serviceMock.Setup(x => x.GetLocations()).Returns(Task.FromResult(typedLocations));

            var pingerMock = new Mock<IExecutePing>();
            pingerMock.Setup(x => x.RoundtripTime).Returns(1L);

            var pingerFactoryMock = new Mock<IExecutePingFactory>();
            pingerFactoryMock.Setup(x => x.Create()).Returns(pingerMock.Object);

            var viewModel = new LocationViewModel(serviceMock.Object, pingerFactoryMock.Object);

            viewModel.RefreshCommand.Execute(null);

            while (viewModel.DataRequestInprogress)
            {

            }

            Assert.NotNull(viewModel.Locations);
            Assert.Equal(3, viewModel.Locations.Count);
        }

        [Fact]
        private void ApiUnuthorizedExceptionTest()
        {
            var serviceMock = new Mock<ILocationService>();
            serviceMock.Setup(x => x.GetLocations()).ThrowsAsync(new UnauthorizedAccessException());

            var pingFactory = new Mock<IExecutePingFactory>();

            var viewModel = new LocationViewModel(serviceMock.Object, pingFactory.Object);

            bool raised = false;
            INotification notification = null;

            viewModel.ApiErrorNotificationRequest.Raised += (s, e) =>
            {
                raised = true;
                notification = e.Context;
            };

            viewModel.RefreshCommand.Execute(null);

            Assert.True(raised);
            Assert.NotNull(notification);
            Assert.Equal(LocationViewModel.API_ERROR_TITLE, notification.Title);
            Assert.Equal(LocationViewModel.API_ERROR_403, notification.Content.ToString());
        }

        [Fact]
        private void ApiTimeOutExceptionTest()
        {
            var serviceMock = new Mock<ILocationService>();
            serviceMock.Setup(x => x.GetLocations()).ThrowsAsync(new TimeoutException());

            var pingFactory = new Mock<IExecutePingFactory>();

            var viewModel = new LocationViewModel(serviceMock.Object, pingFactory.Object);

            bool raised = false;
            INotification notification = null;

            viewModel.ApiErrorNotificationRequest.Raised += (s, e) =>
            {
                raised = true;
                notification = e.Context;
            };

            viewModel.RefreshCommand.Execute(null);

            Assert.True(raised);
            Assert.NotNull(notification);
            Assert.Equal(LocationViewModel.API_ERROR_TITLE, notification.Title);
            Assert.Equal(LocationViewModel.API_ERROR_TIME_OUT, notification.Content.ToString());
        }

        [Fact]
        private void ApiUnknownExceptionTest()
        {
            var serviceMock = new Mock<ILocationService>();
            serviceMock.Setup(x => x.GetLocations()).ThrowsAsync(new UnknownApiException(null, null));

            var pingFactory = new Mock<IExecutePingFactory>();

            var viewModel = new LocationViewModel(serviceMock.Object, pingFactory.Object);

            bool raised = false;
            INotification notification = null;

            viewModel.ApiErrorNotificationRequest.Raised += (s, e) =>
            {
                raised = true;
                notification = e.Context;
            };

            viewModel.RefreshCommand.Execute(null);

            Assert.True(raised);
            Assert.NotNull(notification);
            Assert.Equal(LocationViewModel.API_ERROR_TITLE, notification.Title);
            Assert.Equal(LocationViewModel.API_ERROR_UNKNOWN, notification.Content.ToString());
        }

        [Fact]
        private void BestLocationTest1()
        {
            var mumbaiSever     = IPAddress.Parse("10.0.0.0");
            var bangaloreSever  = IPAddress.Parse("20.0.0.0");
            var delhiSever      = IPAddress.Parse("30.0.0.0");

            var bestPingMock = new Mock<IPing>();
            bestPingMock.Setup(x => x.Ping(mumbaiSever)).Returns(Task.FromResult(100L));

            var secondBestPingMock = new Mock<IPing>();
            secondBestPingMock.Setup(x => x.Ping(bangaloreSever)).Returns(Task.FromResult(200L));

            var thirdBestPingMock = new Mock<IPing>();
            thirdBestPingMock.Setup(x => x.Ping(delhiSever)).Returns(Task.FromResult(300L));            

            var serviceMock = new Mock<ILocationService>();
            var pingerFactoryMock = new Mock<IExecutePingFactory>();            

            var viewModel = new LocationViewModel(serviceMock.Object, pingerFactoryMock.Object);

            var bestLocation = new LocationModel(new ExecutePingAsync(bestPingMock.Object));
            bestLocation.LocationPingChanged += viewModel.Location_LocationPingChanged;
            bestLocation.Name = "India - Mumbai";
            bestLocation.Server = new Server() { IP = mumbaiSever };

            var secondBestLocation = new LocationModel(new ExecutePingAsync(secondBestPingMock.Object));
            secondBestLocation.LocationPingChanged += viewModel.Location_LocationPingChanged;
            secondBestLocation.Name = "India - Bangalore";
            secondBestLocation.Server = new Server() { IP = bangaloreSever };

            var thirdBestLocation = new LocationModel(new ExecutePingAsync(thirdBestPingMock.Object));
            thirdBestLocation.LocationPingChanged += viewModel.Location_LocationPingChanged;
            thirdBestLocation.Name = "India - Delhi";
            thirdBestLocation.Server = new Server() { IP = delhiSever };

            viewModel.Locations.Add(bestLocation);
            viewModel.Locations.Add(secondBestLocation);
            viewModel.Locations.Add(thirdBestLocation);

            Assert.NotNull(viewModel.CurrentBestLocation);
            Assert.Equal(100L, viewModel.CurrentBestLocation.Ping);
            Assert.Equal("India - Mumbai", viewModel.CurrentBestLocation.LocationName);
        }

        [Fact]
        private void BestLocationTest2()
        {
            var mumbaiSever = IPAddress.Parse("10.0.0.0");
            var bangaloreSever = IPAddress.Parse("20.0.0.0");
            var delhiSever = IPAddress.Parse("30.0.0.0");

            var bestPingMock = new Mock<IPing>();
            bestPingMock.Setup(x => x.Ping(mumbaiSever)).Returns(Task.FromResult(100L));

            var secondBestPingMock = new Mock<IPing>();
            secondBestPingMock.Setup(x => x.Ping(bangaloreSever)).Returns(Task.FromResult(20L));

            var thirdBestPingMock = new Mock<IPing>();
            thirdBestPingMock.Setup(x => x.Ping(delhiSever)).Returns(Task.FromResult(300L));

            var serviceMock = new Mock<ILocationService>();
            var pingerFactoryMock = new Mock<IExecutePingFactory>();

            var viewModel = new LocationViewModel(serviceMock.Object, pingerFactoryMock.Object);

            var bestLocation = new LocationModel(new ExecutePingAsync(bestPingMock.Object));
            bestLocation.LocationPingChanged += viewModel.Location_LocationPingChanged;
            bestLocation.Name = "India - Mumbai";
            bestLocation.Server = new Server() { IP = mumbaiSever };

            var secondBestLocation = new LocationModel(new ExecutePingAsync(secondBestPingMock.Object));
            secondBestLocation.LocationPingChanged += viewModel.Location_LocationPingChanged;
            secondBestLocation.Name = "India - Bangalore";
            secondBestLocation.Server = new Server() { IP = bangaloreSever };

            var thirdBestLocation = new LocationModel(new ExecutePingAsync(thirdBestPingMock.Object));
            thirdBestLocation.LocationPingChanged += viewModel.Location_LocationPingChanged;
            thirdBestLocation.Name = "India - Delhi";
            thirdBestLocation.Server = new Server() { IP = delhiSever };

            viewModel.Locations.Add(bestLocation);
            viewModel.Locations.Add(secondBestLocation);
            viewModel.Locations.Add(thirdBestLocation);

            Assert.NotNull(viewModel.CurrentBestLocation);
            Assert.Equal(20L, viewModel.CurrentBestLocation.Ping);
            Assert.Equal("India - Bangalore", viewModel.CurrentBestLocation.LocationName);
        }

        [Fact]
        private void BestLocationTest3()
        {
            var mumbaiSever = IPAddress.Parse("10.0.0.0");
            var bangaloreSever = IPAddress.Parse("20.0.0.0");
            var delhiSever = IPAddress.Parse("30.0.0.0");

            var bestPingMock = new Mock<IPing>();
            bestPingMock.Setup(x => x.Ping(mumbaiSever)).Returns(Task.FromResult(100L));

            var secondBestPingMock = new Mock<IPing>();
            secondBestPingMock.Setup(x => x.Ping(bangaloreSever)).Returns(Task.FromResult(200L));

            var thirdBestPingMock = new Mock<IPing>();
            thirdBestPingMock.Setup(x => x.Ping(delhiSever)).Returns(Task.FromResult(30L));

            var serviceMock = new Mock<ILocationService>();
            var pingerFactoryMock = new Mock<IExecutePingFactory>();

            var viewModel = new LocationViewModel(serviceMock.Object, pingerFactoryMock.Object);

            var bestLocation = new LocationModel(new ExecutePingAsync(bestPingMock.Object));
            bestLocation.LocationPingChanged += viewModel.Location_LocationPingChanged;
            bestLocation.Name = "India - Mumbai";
            bestLocation.Server = new Server() { IP = mumbaiSever };

            var secondBestLocation = new LocationModel(new ExecutePingAsync(secondBestPingMock.Object));
            secondBestLocation.LocationPingChanged += viewModel.Location_LocationPingChanged;
            secondBestLocation.Name = "India - Bangalore";
            secondBestLocation.Server = new Server() { IP = bangaloreSever };

            var thirdBestLocation = new LocationModel(new ExecutePingAsync(thirdBestPingMock.Object));
            thirdBestLocation.LocationPingChanged += viewModel.Location_LocationPingChanged;
            thirdBestLocation.Name = "India - Delhi";
            thirdBestLocation.Server = new Server() { IP = delhiSever };

            viewModel.Locations.Add(bestLocation);
            viewModel.Locations.Add(secondBestLocation);
            viewModel.Locations.Add(thirdBestLocation);

            Assert.NotNull(viewModel.CurrentBestLocation);
            Assert.Equal(30L, viewModel.CurrentBestLocation.Ping);
            Assert.Equal("India - Delhi", viewModel.CurrentBestLocation.LocationName);
        }

        [Fact]
        private void CallToRefreshCommandTest1()
        {
            var serviceMock = new Mock<ILocationService>();
            serviceMock.Setup(x => x.GetLocations()).Returns(async () =>
            {
                await Task.Delay(2000);
                return new XvpnLocations();
            });

            var pingerFactoryMock = new Mock<IExecutePingFactory>();

            var viewModel = new LocationViewModel(serviceMock.Object, pingerFactoryMock.Object);

            Assert.False(viewModel.DataRequestInprogress);
            viewModel.RefreshCommand.Execute(null);
            Assert.True(viewModel.DataRequestInprogress);
        }

        [Fact]
        private void CallToRefreshCommandTest2()
        {
            var serviceMock = new Mock<ILocationService>();
            serviceMock.Setup(x => x.GetLocations()).Returns(async () =>
            {
                await Task.Delay(3000);
                return new XvpnLocations();
            });

            var pingerFactoryMock = new Mock<IExecutePingFactory>();

            var viewModel = new LocationViewModel(serviceMock.Object, pingerFactoryMock.Object);

            Assert.False(viewModel.DataRequestInprogress);
            viewModel.RefreshCommand.Execute(null);
            Assert.True(viewModel.DataRequestInprogress);
            viewModel.RefreshCommand.Execute(null);
            Assert.True(viewModel.DataRequestInprogress);
            viewModel.RefreshCommand.Execute(null);
            Assert.True(viewModel.DataRequestInprogress);
        }

    }
}