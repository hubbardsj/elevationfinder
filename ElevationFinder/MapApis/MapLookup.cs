using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using Windows.Web.Http;

namespace ElevationFinder.MapApis
{
    //todo:  create interface for testing
    public class MapLookup
    {
        public async Task<HttpResponseMessage> FindCoordsByAddress(string address)
        {
            var client = EndpointClient.GetHttpClient();
            return await client.GetAsync(GetGeocodeEndpoint(address));
        }

        public async Task<double> GetElevationFromMapProvider(double lat, double longtitude)
        {
            var elevationInfo = await EndpointClient.GetHttpClient().GetAsync(GetElevationEndpoint(lat.ToString(), longtitude.ToString()), HttpCompletionOption.ResponseContentRead);
            var mapquestElevation = JsonConvert.DeserializeObject<MapquestElevation>(elevationInfo.Content.ToString());

            return double.Parse(mapquestElevation.ElevationProfile[0].Height) * 3.3;    //convert to feet and return.
        }
        internal async Task<double> GetElevationFromMapProviderFromEnteredLocation(string userEnteredLocation)
        {
            var result = await FindCoordsByAddress(userEnteredLocation);
            var location = JsonConvert.DeserializeObject<LocationLookup>(result.Content.ToString());
            return await GetElevationFromMapProvider(double.Parse(location.results[0].locations[0].latLng.lat), double.Parse(location.results[0].locations[0].latLng.lng));
        }

        private Uri GetGeocodeEndpoint(string address)
        {              
            return new Uri(string.Format(MapquestGeocodeEndpoint, Secrets.MapQuestAPIKey, address));
        }

        private Uri GetElevationEndpoint(string latitude, string longitude)
        {
                return new Uri(string.Format(MapquestElevationEndpoint, Secrets.MapQuestAPIKey, latitude, longitude));
        }

        private const string MapquestGeocodeEndpoint = "http://www.mapquestapi.com/geocoding/v1/address?key={0}&location={1}";
        private const string MapquestElevationEndpoint = "http://open.mapquestapi.com/elevation/v1/profile?key={0}&shapeFormat=raw&latLngCollection={1},{2}";


    }
}
