using Windows.Networking.Connectivity;

namespace ElevationFinder
{
    public class InternetCheck
    {
        public static bool HasInternetAccess()
        {
            var connection = NetworkInformation.GetInternetConnectionProfile();
            return connection != null && connection.GetNetworkConnectivityLevel() == NetworkConnectivityLevel.InternetAccess;
        }

    }
}
