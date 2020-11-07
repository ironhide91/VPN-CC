using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Vpn.UI.LocationModule
{
    public interface ILocationViewModel
    {
        bool DataRequestInprogress { get; }

        string ButtonText { get; }

        ObservableCollection<LocationModel> Locations { get; }

        LocationPingChanged CurrentBestLocation { get; }

        ICommand RefreshCommand { get; }

        ICommand ShowBestLocationCommand { get; }
    }
}