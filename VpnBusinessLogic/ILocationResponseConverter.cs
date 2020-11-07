using Vpn.BusinessLogic.DataModels;

namespace Vpn.BusinessLogic
{
    public interface ILocationResponseConverter
    {
        VpnLocations Convert(string payload);
    }
}