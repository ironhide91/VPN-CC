using System;
using System.Net;

namespace Xvpn.UI.Helper
{
    public interface IExecutePing
    {
        IPAddress IPAddress { get; set; }

        double PingInterval { get; set; }

        long RoundtripTime { get; }

        void Execute();

        event EventHandler<PingChangedEventArgs> PingChanged;
    }
}