using System.Net;
using System.Threading.Tasks;

namespace Vpn.UI.Helper
{
    public interface IPing
    {
        Task<long> Ping(IPAddress ipAddress);        
    }
}