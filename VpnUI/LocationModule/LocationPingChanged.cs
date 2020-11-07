using System.Net;
using System.Windows.Media.Imaging;

namespace Vpn.UI.LocationModule
{
    public class LocationPingChanged
    {
        public BitmapImage Icon { get; set; }

        public string LocationName { get; set; }

        public IPAddress Server { get; set; }

        public double Ping { get; set; }
    }
}