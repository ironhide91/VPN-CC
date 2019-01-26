using System;
using System.Threading.Tasks;
using Xvpn.BusinessLogic.DataModels;

namespace Xvpn.BusinessLogic
{
    public interface ILocationService
    {
        TimeSpan Throttle { get; set;  }

        Uri ApiUrl { get; set; }

        Task<XvpnLocations> GetLocations();
    }
}