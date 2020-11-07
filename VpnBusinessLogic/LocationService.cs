using System;
using System.Net.Http;
using System.Threading.Tasks;
using Vpn.BusinessLogic.DataModels;

namespace Vpn.BusinessLogic
{
    public class LocationService : ILocationService
    {
        private ICustomHttpClient _Httpclient;
        private ILocationResponseConverter _Converter;

        public LocationService(ICustomHttpClient httpclient, ILocationResponseConverter converter)
        {
            _Httpclient = httpclient;
            _Converter = converter;
        }

        public TimeSpan Throttle { get; set; }

        public Uri ApiUrl { get; set; }

        public async Task<VpnLocations> GetLocations()
        {
            var httpResponse = await _Httpclient.Execute(ApiUrl, HttpVerb.Get);

            httpResponse.EnsureSuccessStatusCode();

            var contentAsString = await httpResponse.Content.ReadAsStringAsync();

            var typedLocationResponse = _Converter.Convert(contentAsString);

            return typedLocationResponse;         
        }        
    }
}