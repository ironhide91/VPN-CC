﻿using System.Net;
using System.Windows.Media.Imaging;

namespace Xvpn.UI.Helper
{
    public class PingChanged
    {
        public IPAddress Server { get; set; }

        public double Ping { get; set; }
    }
}