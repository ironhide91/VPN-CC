using Prism.Mvvm;
using System.Windows.Media.Imaging;
using Xvpn.BusinessLogic.DataModels;
using Xvpn.UI.Helper;
using System;

namespace Xvpn.UI.LocationModule
{
    public class LocationModel : BindableBase
    {
        public LocationModel(IExecutePing pingExecuter)
        {
            PingExecuter = pingExecuter ?? throw new ArgumentNullException(nameof(pingExecuter)); ;
            PingExecuter.PingChanged += PingExecuter_PingChanged; ;
        }

        private void PingExecuter_PingChanged(object sender, PingChangedEventArgs e)
        {
            var location = new LocationPingChanged();
            location.Icon = Icon;
            location.LocationName = Name;
            location.Server = e.NewPing.Server;
            location.Ping = e.NewPing.Ping;

            var args = new LocationPingChangedEventArgs();
            args.NewLocationPing = location;

            LocationPingChanged?.Invoke(this, args);
        }

        public event EventHandler<LocationPingChangedEventArgs> LocationPingChanged;

        public IExecutePing PingExecuter { get; private set; }

        private string _Name;
        public string Name
        {
            get { return _Name; }
            set
            {
                _Name = value;
                RaisePropertyChanged(nameof(Name));
            }
        }

        private int _SortOrder;
        public int SortOrder
        {
            get { return _SortOrder; }
            set
            {
                _SortOrder = value;
                RaisePropertyChanged(nameof(SortOrder));
            }
        }

        private BitmapImage _Icon;
        public BitmapImage Icon
        {
            get { return _Icon; }
            set
            {
                _Icon = value;
                RaisePropertyChanged(nameof(Icon));
            }
        }

        private Server _Server;
        public Server Server
        {
            get { return _Server; }
            set
            {
                _Server = value;
                RaisePropertyChanged(nameof(Server));
                PingExecuter.IPAddress = _Server.IP;
                PingExecuter.Execute();
            }
        }
    }
}