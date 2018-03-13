using System;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.Foundation.Metadata;

namespace ElevationFinder.GeolocationService
{
    //GPS : within approximately 10 meters
    //Wi-Fi : between approximately 30 meters and 500 meters
    //Cell towers : between approximately 300 meters and 3,000 meters
    //IP address : between approximately 1,000 meters and 5,000 meters. 

    public interface IDeviceLocationLookup
    {
        Task<LocationInformation> GetLocationInfo();
    }
    public class DeviceLocationLookup : IDeviceLocationLookup
    {
        async public Task<LocationInformation> GetLocationInfo()
        {
            var locator = new Geolocator() { DesiredAccuracy = PositionAccuracy.High };
            var position = await locator.GetGeopositionAsync();
            return new LocationInformation()
            {
                ElevationInMeters = position.Coordinate.Point.Position.Altitude,
                Latitude = position.Coordinate.Point.Position.Latitude,
                Longitude = position.Coordinate.Point.Position.Longitude,
                ElevationInMetersAccuracy = position.Coordinate.AltitudeAccuracy,
                LatLongAccuracyInMeters = position.Coordinate.Accuracy
            };
        }
    }


}
