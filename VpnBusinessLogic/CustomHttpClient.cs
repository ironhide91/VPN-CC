using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Vpn.BusinessLogic
{
    public class CustomHttpClient : ICustomHttpClient
    {
        public CustomHttpClient()
        {
            _HttpClient = new HttpClient();
        }

        ~CustomHttpClient()
        {
            Dispose(false);
        }

        public async Task<HttpResponseMessage> Execute(Uri url, HttpVerb httpVerb, HttpContent content = null)
        {
            HttpResponseMessage response = null;

            try
            {
                switch (httpVerb)
                {
                    case HttpVerb.Get:
                        response = await _HttpClient.GetAsync(url).ConfigureAwait(false);
                        break;
                    case HttpVerb.Post:
                        throw new NotImplementedException("Post not implemented yet !");
                    default:
                        break;
                }
            }
            catch (Exception e) when (IsUnknownApiException(e))
            {
                throw new UnknownApiException(e.Message, e);
            }            

            return response;
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
            _HttpClient.Dispose();

            _Disposed = true;
        }

        private bool _Disposed = false;

        private static Func<Exception, bool> IsUnknownApiException =>
            x => (x.GetType() != typeof(UnauthorizedAccessException) &&
                  x.GetType() != typeof(TimeoutException));

        private readonly HttpClient _HttpClient;
    }
}