using Windows.Devices.Geolocation;

namespace ElevationFinder
{
    /// <summary>
    /// This class represents the easier-to-digest (and fields we actually care about) representation of a Geoposition/Location.
    /// Used as a translation + conversion (units:  feet/meters, for example) layer around the raw data.
    /// </summary>
    public class LocationInformation 
    {
        public double ElevationInFeet { get { return ElevationInMeters * 3.3; } }
        public double ElevationInMeters { get; set; }
        public double? ElevationInFeetAccuracy { get { return ElevationInMetersAccuracy * 3.3; } }  //devices and provider lookups always return meters.  feet should always be derived.
        public double? ElevationInMetersAccuracy { get; set; }

        public double Latitude { get; set;  }
        public double Longitude { get; set; }

        public double LatLongAccuracyInFeet { get { return LatLongAccuracyInMeters * 3.3; } }
        public double LatLongAccuracyInMeters { get; set; }

        public static LocationInformation FromGeolocationPosition(Geoposition position)
        {
            return new LocationInformation
            {
                ElevationInMeters = position.Coordinate.Point.Position.Altitude,
                ElevationInMetersAccuracy = position.Coordinate.AltitudeAccuracy,
                Latitude = position.Coordinate.Point.Position.Latitude,
                Longitude = position.Coordinate.Point.Position.Longitude,
                LatLongAccuracyInMeters = position.Coordinate.Accuracy
            };
        }
    }
}
