using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;
using Xamarin.Forms.Xaml;

namespace otp.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ServiceMapPage : Controls.Custompage
    {
        public ServiceMapPage()
        {
            InitializeComponent();
            //myMap.MoveToRegion(MapSpan.FromCenterAndRadius(
            //new Position(26.5590737, 31.6956705), Distance.FromMiles(1)));


            //var position = new Position(26.5590737, 31.6956705);
            //var pin = new Pin
            //{
            //    Type = PinType.Place,
            //    Address = "work at home",
                
            //    Position = position,
            //    Label = "hi"


            //};
            //myMap.Pins.Add(pin);

        }

        //private async void SearchButton_Clicked(object sender, EventArgs e)
        
        //{
        //    // Assuming you have a search text input named searchTextEntry
        //    string searchText = searchBar.Text;

        //    // Perform geocoding to convert search text to coordinates
        //    // Here you would typically use a geocoding service or API
        //    // For demonstration purposes, let's assume we have a hardcoded result
        //    try
        //    {
        //        var locations = await Geocoding.GetLocationsAsync(searchText);
        //        var location = locations?.FirstOrDefault();

        //        if (location != null)
        //        {
        //            Position searchResultPosition = new Position(location.Latitude, location.Longitude);

        //            // Update map region based on search result
        //            myMap.MoveToRegion(MapSpan.FromCenterAndRadius(
        //                searchResultPosition, Distance.FromMiles(1)));
        //        }
        //        else
        //        {
        //            // Handle case where no location is found
        //            await DisplayAlert("Error", "Location not found", "OK");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        // Handle geocoding exception
        //        await DisplayAlert("Error", "Failed to geocode location", "OK");
        //    }
        //}
    
    }
}