using Windows.Web.Http;

namespace ElevationFinder.MapApis
{
    public sealed class EndpointClient
    {

        private EndpointClient()
        {

        }

        public static HttpClient GetHttpClient()
        {
            return _instance;
        }

        private static readonly HttpClient _instance = new HttpClient();
    }
}
