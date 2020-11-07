using System;
using System.Threading.Tasks;
using Vpn.BusinessLogic.DataModels;

namespace Vpn.BusinessLogic
{
    public interface ILocationService
    {
        TimeSpan Throttle { get; set;  }

        Uri ApiUrl { get; set; }

        Task<VpnLocations> GetLocations();
    }
}