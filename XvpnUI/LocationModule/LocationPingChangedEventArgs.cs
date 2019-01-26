using System;

namespace Xvpn.UI.LocationModule
{
    public class LocationPingChangedEventArgs : EventArgs
    {
        public LocationPingChanged NewLocationPing { get; set; }
    }
}