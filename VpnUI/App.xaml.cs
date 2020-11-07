using Prism.Ioc;
using Prism.Modularity;
using Prism.Unity;
using System.Windows;
using Vpn.UI.LocationModule;

namespace Vpn.UI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : PrismApplication
    {
        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {

        }

        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            moduleCatalog.AddModule<LocationPrismModule>();
        }
    }
}