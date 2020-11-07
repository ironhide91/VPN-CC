using System;
using System.Net;
using System.Net.NetworkInformation;
using System.Threading.Tasks;

namespace Vpn.UI.Helper
{
    public class DefaultPing : IPing, IDisposable
    {
        private Ping _Ping;
        private bool _Disposed = false;

        public DefaultPing()
        {
            _Ping = new Ping();
        }

        public async Task<long> Ping(IPAddress ipAddress)
        {
            var pingReply = await _Ping.SendPingAsync(ipAddress);
            return pingReply.RoundtripTime;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_Disposed)
                return;

            if (disposing)
            {
                //- Managed Resource
            }

            //- UnManaged Resource
            _Ping.Dispose();

            _Disposed = true;
        }

        ~DefaultPing()
        {
            Dispose(false);
        }
    }
}