using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using otp.Models;
using FreshMvvm;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;
using Xamarin.Forms.GoogleMaps.Bindings;
using Rg.Plugins.Popup.Services;

namespace otp.ViewModels
{
  public  class ServiceMapViewModel : FreshBasePageModel
    {
        public Command SearchWithText { get; set; }
        public Command PinClickCommand { get; set; }
        public Command Discard { get; set; }
        public Command RequestService { get; set; }
        public MoveToRegionRequest Request { get; set; } = new MoveToRegionRequest();

        string _SearchTerm;
        public string SearchTerm
        {
            get
            {
                return _SearchTerm;
            }
            set
            {
                _SearchTerm = value;

                RaisePropertyChanged("SearchTerm");
            }
        }

        ObservableCollection<ProviderModel> _ListProviders;
        public ObservableCollection<ProviderModel> ListProviders
        {

            get
            {
                return _ListProviders;
            }
            set
            {
                _ListProviders = value;
                RaisePropertyChanged("ListProviders");
            }
        }
        ObservableCollection<Pin> _ProviderPins;
        public ObservableCollection<Pin> ProviderPins
        {

            get
            {
                return _ProviderPins;
            }
            set
            {
                _ProviderPins = value;
                RaisePropertyChanged("ProviderPins");
            }
        }

        public ServiceModel CurrentService { get; set; }
        public ProviderModel CurrentProvider { get; set; }

        public users CurrentUser { get; set; }

        string _PageTitle;
        public string PageTitle
        {
            get
            {
                return _PageTitle;

            }
            set
            {
                _PageTitle = value;
                RaisePropertyChanged("PageTitle");

            }
        }
       
        public ServiceMapViewModel(ServiceModel service)
        {
            SearchWithText = new Command(OnSearchWithText);
            ProviderPins = new ObservableCollection<Pin>();
            PinClickCommand = new Command<PinClickedEventArgs>(OnPinClick);
            RequestService = new Command(OnRequestService);
            Discard = new Command(OnDiscard);
        }
        public ServiceMapViewModel()
        {

        }

        
        async void OnPinClick(PinClickedEventArgs e)
        {
            var providerId = Convert.ToInt32( e.Pin.Tag);
           
            CurrentProvider = ListProviders.Where(a => a.ProvicderId == providerId).FirstOrDefault();
            if (Helpers.Settings.Language == "ar")
            {
                CurrentProvider.ServiceName = CurrentService.ServiceNameAr;
                CurrentProvider.userId = CurrentService.userId;
            }
            else
            {
                CurrentProvider.ServiceName = CurrentService.ServiceName;
                CurrentProvider.userId = CurrentService.userId;
            }
            var page = new Pages.ProviderPopUp();
            page.BindingContext = this;
            await PopupNavigation.PushAsync(page);

        }

        IServiceProvider _service;
        public ServiceMapViewModel(IServiceProvider service)
        {
            _service = service;
        }

        public async override void Init(object initData)
        {
            base.Init(initData);

            
            CurrentService = initData as ServiceModel;
            PageTitle = CurrentService.ServiceName;
            var _provider = new Services.ServicesProviders();
            ListProviders = await _provider.GetProviders(CurrentService.ServiceId);
            foreach (var provider in ListProviders)
            {
                var position = new Position(provider.Lat, provider.Long);
                var pin = new Pin
                {
                    Type = PinType.Place,
                    Label = provider.ProviderName,

                    Address = provider.Description,

                    Position = position,
                    Tag=provider.ProvicderId



                };
                ProviderPins.Add(pin);
            }

        }

      
        
       
       async void OnSearchWithText()
        {
            try
            
            {
                var locations = await Geocoding.GetLocationsAsync(SearchTerm);
                var location = locations?.FirstOrDefault();

                if (location != null)
                {
                    var request = new GeolocationRequest(GeolocationAccuracy.Medium);
                  //  var location = await Geolocation.GetLocationAsync(request);
                    Position searchResultPosition = new Position(location.Latitude, location.Longitude);

                    // Update map region based on search result
                    Request.MoveToRegion(MapSpan.FromCenterAndRadius(
                        searchResultPosition, Distance.FromMiles(1)));
                }
                else
                {
                    // Handle case where no location is found
                    //await  DisplayAlert("Error", "Location not found", "OK");
                }
            }
            catch (Exception ex)
            {
                // Handle geocoding exception
              await CoreMethods.DisplayAlert("Error", "Failed to geocode location", "OK");
            }
        
    }
        //protected async override void ViewIsAppearing(object sender, EventArgs e)
        //{


        //    var request = new GeolocationRequest(GeolocationAccuracy.Medium);
        //    var location = await Geolocation.GetLocationAsync(request);
        //    Request.MoveToRegion(MapSpan.FromCenterAndRadius(
        //    new Position(26.5590737, 31.6956705), Distance.FromMiles(1)));
        //    base.ViewIsAppearing(sender, e);

        //}
        protected async override void ViewIsAppearing(object sender, EventArgs e)
        {


            var request = new GeolocationRequest(GeolocationAccuracy.Medium);
            var location = await Geolocation.GetLocationAsync(request);

            if (location != null)
            {
                Request.MoveToRegion(MapSpan.FromCenterAndRadius(
                    new Position(location.Latitude, location.Longitude), Distance.FromMiles(1)));
            }
            else
            {
                // Handle case where location is null (user denied permission, or location services are disabled)
                // You can show a message to the user or take appropriate action.
            }
            base.ViewIsAppearing(sender, e);
        }


        async void OnDiscard()
        {
            await PopupNavigation.PopAsync();
            // await CoreMethods.PushPageModel<RequestViewModel>(SelectedProvider);
        }
        async void OnRequestService()
        {

            await PopupNavigation.PopAsync();
            CurrentProvider.ServiceId = CurrentService.ServiceId;
            CurrentProvider.ServiceName = CurrentService.ServiceName;
            CurrentProvider.ServiceNameAr = CurrentService.ServiceNameAr;
           
            await CoreMethods.PushPageModel<RequsetViewModel>(CurrentProvider,modal:true);
            
        }



    }
    public class RequestViewModelParameters
    {
        public ProviderModel CurrentProvider { get; set; }
        public users CurrentUser { get; set; }
    }

}
