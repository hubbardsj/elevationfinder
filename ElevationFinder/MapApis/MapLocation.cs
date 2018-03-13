

namespace ElevationFinder.MapApis
{
    //Not my favorite way to name things, but this provides a very quick and easy way to serialize/deserialize MapQuest results into something usable

    public class LocationLookup
    {
        public Results[] results { get; set; }
    }

    public class Results
    {
        public MapquestLocation[] locations { get; set; }

    }
    public class MapquestLocation
    {
        public latLng latLng { get; set; }
    }

    public class latLng
    {
        public string lat { get; set; }
        public string lng { get; set; }
    }


    public class MapquestElevation
    {
        public ElevationProfile[] ElevationProfile { get; set; }
    }

    public class ElevationProfile
    {
        public string Height { get; set; }
    }
}
