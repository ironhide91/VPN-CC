using Xvpn.BusinessLogic.DataModels;

namespace Xvpn.BusinessLogic
{
    public interface ILocationResponseConverter
    {
        XvpnLocations Convert(string payload);
    }
}