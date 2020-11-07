using System;

namespace Vpn.UI.LocationModule
{
    public class LocationPingChangedEventArgs : EventArgs
    {
        public LocationPingChanged NewLocationPing { get; set; }
    }
}