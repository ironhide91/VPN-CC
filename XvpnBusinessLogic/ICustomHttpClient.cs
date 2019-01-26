using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Xvpn.BusinessLogic
{
    public interface ICustomHttpClient : IDisposable
    {
        Task<HttpResponseMessage> Execute(Uri url, HttpVerb httpVerb, HttpContent content = null);
    }
}
