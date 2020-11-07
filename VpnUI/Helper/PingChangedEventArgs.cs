using System;

namespace Vpn.UI.Helper
{
    public class PingChangedEventArgs : EventArgs
    {
        public PingChanged NewPing { get; set; }
    }
}