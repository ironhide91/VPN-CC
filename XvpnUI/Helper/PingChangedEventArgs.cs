using System;

namespace Xvpn.UI.Helper
{
    public class PingChangedEventArgs : EventArgs
    {
        public PingChanged NewPing { get; set; }
    }
}