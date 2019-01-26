using System;
using Xvpn.UI.Helper;

namespace Xvpn.UI.LocationModule
{
    public class ExecutePingFactory : IExecutePingFactory
    {
        private Func<IExecutePing> _IExecutePingResolver;

        public ExecutePingFactory(Func<IExecutePing> pinger)
        {
            _IExecutePingResolver = pinger ?? throw new ArgumentNullException(nameof(pinger));
        }

        public IExecutePing Create()
        {
            return _IExecutePingResolver();
        }
    }
}