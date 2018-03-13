using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using ElevationFinder.MapApis;
using Newtonsoft.Json;
using System;
using Windows.ApplicationModel;


//TODOs:
//1.  Integrate with Cortana.  "What is my elevation?"
//2.  Have an auto-update - requery the elevation every X minutes.
//3.  Integrate with other map providers?  Likely little gains there.
//4.  Detect GPS and internet - update UI for features available given what's available.
//5.  Start page to request location permissions for app.
//6.  Support standard/metric
//7.  On startup, immediate query GPS to display something faster.
//8.  Check for inet before doing stuff - display message if inet doesn't exist.
//9.  Do the math calcs (convert to feet) elsewhere
//10. Make the UI/UX not quite as terrible.

//Consumer Key    v9GPlTijPHIfVMpDRR3OGD5gDZJXliay
//Consumer Secret oP6NnKotS0rtIyyv

namespace ElevationFinder
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {

            InitializeComponent();
            SetVersion();
            UpdateElevation();
        }

        private void SetVersion()
        {
            textVersion.Text = string.Format(VersionInformation,
                 Package.Current.Id.Version.Major,
                 Package.Current.Id.Version.Minor,
                 Package.Current.Id.Version.Build,
                 Package.Current.Id.Version.Revision);
        }

        private void buttonElevation_Click(object sender, RoutedEventArgs e)
        {
            UpdateElevation();
        }

        private async void UpdateElevation(Object referenceData = null)
        {
            try
            {
                UpdateGPSElevationLabelsForWaitingForData();
                var locationInfo = await new GeolocationService.DeviceLocationLookup().GetLocationInfo();
                UpdateGPSElevationLabel(locationInfo);
                UpdateMapElevationFromGPSPosition(locationInfo);
            }
            catch (Exception ex)
            {
                textException.Text = ex.Message;
            }
        }

        private async void UpdateMapElevationFromGPSPosition(LocationInformation locationInfo)
        {
            DisplayMapsElevationLabel2(await new MapLookup().GetElevationFromMapProvider(locationInfo.Latitude, locationInfo.Longitude));
        }
        private void UpdateGPSElevationLabel(LocationInformation locationInfo)
        {

            var gpsInfo = string.Format(GPSLocationInformation,
              Environment.NewLine,
              locationInfo.Latitude,
              Environment.NewLine,
              locationInfo.Longitude,
              Environment.NewLine,
              locationInfo.LatLongAccuracyInFeet,
              Feet);
            var elevation = string.Format(ElevationText, locationInfo.ElevationInFeet, Feet);
            var elevationKey = string.Format(GPSElevationKey, locationInfo.ElevationInFeetAccuracy, Feet);

            DisplayGPSElevationLabels(elevation, elevationKey, gpsInfo, GPSQueryingText);
        }

        private void UpdateGPSElevationLabelsForWaitingForData()
        {
            DisplayGPSElevationLabels(GPSLoadingText, string.Empty, GPSQueryingText, GPSLoadingText);
        }
        private void DisplayGPSElevationLabels(string elevation, string elevationKey, string gpsInformation, string mapLocation)
        {
            textGPSElevation.Text = elevation;
            textGPSKey.Text = elevationKey;
            textLatLong.Text = gpsInformation;
            textGPSMapElevation.Text = mapLocation;
        }

        private void DisplayMapsElevationLabel2(double elevation)
        {
            textGPSMapElevation.Text = string.Format(ElevationText, elevation, Feet);
        }

        private async void buttonFindCityState_Click(object sender, RoutedEventArgs e)
        {  
            try
            {
                var mapProviderElevation = await new MapLookup().GetElevationFromMapProviderFromEnteredLocation(UserEnteredLocation);
                DisplayMapsElevationLabel2(mapProviderElevation);
                UpdateGPSElevationLabelForMapData();
            }
            catch (Exception ex)
            {
                textException.Text = ex.ToString();
            }
        }


        private void UpdateGPSElevationLabelForMapData()
        {
            textLatLong.Text = "Latest results from map lookup." +Environment.NewLine +"GPS data not used.";
            textGPSElevation.Text = "-- feet (latest results from map lookup.";
            textGPSKey.Text = "Lookup from Mapquest - GPS data not used.";
        }

        private string UserEnteredLocation
        {
            get
            {   
                return textCityState.Text.Trim();   //validation and prettifying can come another time.
            }
        }


        private const string InvalidLocation = "Unknown location entered.";
        private const string LookingForLocation = "Looking up {0}";
        private const string VersionInformation = "v{0}.{1}.{2}.{3}";
        private const string GPSLocationInformation = "Device GPS information:{0} Latitude: {1} {2} Longitude: {3} {4} Accuracy ± {5} {6}";
        private const string Feet = "feet";
        private const string ElevationText = "{0} {1}";
        private const string GPSElevationKey = "Elevation from GPS.Accuracy:  ± {0} {1}";
        private const string GPSLoadingText = "Loading...";
        private const string GPSQueryingText = "Querying device GPS...";
    }
}
