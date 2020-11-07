using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using Prism.Unity;
using System;
using Unity;
using Unity.Lifetime;
using Vpn.BusinessLogic;
using Vpn.UI.Helper;

namespace Vpn.UI.LocationModule
{
    public class LocationPrismModule : IModule
    {
        public const string REGION_NAME = "LocationRegion";

        private static TimeSpan _Throttle = TimeSpan.FromSeconds(3000);

        private static Uri _ApiUrl = new Uri("https://private-anon-966d5878ae-xvjune2014trial.apiary-mock.com/locations");

        public void OnInitialized(IContainerProvider containerProvider)
        {
            var locationService = containerProvider.Resolve<ILocationService>();
            locationService.Throttle = _Throttle;
            locationService.ApiUrl = _ApiUrl;

            var regionManager = containerProvider.Resolve<IRegionManager>();
            regionManager.RegisterViewWithRegion(REGION_NAME, typeof(LocationView));
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            var container = containerRegistry.GetContainer();

            container.RegisterType<IPing, DefaultPing>(new TransientLifetimeManager());
            container.RegisterType<IExecutePing, ExecutePingAsync>(new TransientLifetimeManager());
            container.RegisterType<LocationModel>(new TransientLifetimeManager());

            var executePingFactory = new ExecutePingFactory(() => container.Resolve<IExecutePing>());
            container.RegisterInstance<IExecutePingFactory>(executePingFactory);

            container.RegisterType<ILocationResponseConverter, XmlLocationResponseConverter>(new ContainerControlledLifetimeManager());
            container.RegisterType<ICustomHttpClient, CustomHttpClient>(new ContainerControlledLifetimeManager());
            container.RegisterType<ILocationService, LocationService>(new ContainerControlledLifetimeManager());
            container.RegisterType<ILocationViewModel, LocationViewModel>(new ContainerControlledLifetimeManager());
        }
    }
}