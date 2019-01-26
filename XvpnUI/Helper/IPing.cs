using System.Net;
using System.Threading.Tasks;

namespace Xvpn.UI.Helper
{
    public interface IPing
    {
        Task<long> Ping(IPAddress ipAddress);        
    }
}