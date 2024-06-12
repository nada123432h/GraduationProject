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
using otp.Services;
using Rg.Plugins.Popup.Services;
using Newtonsoft.Json;
using System.IO;
using Rg.Plugins.Popup.Extensions;
using Acr.UserDialogs;
using System.Threading.Tasks;

namespace otp.ViewModels
{
  public  class RequsetViewModel : FreshBasePageModel
    {
        string _Description;
        public string Description
        {
            get
            {
                return _Description;
            }
            set
            {
                _Description = value;
                RaisePropertyChanged("Description");

            }
        }
        private int _UserId;
        public int UserId
        {
            get => _UserId;
            set
            {
                if (_UserId != value)
                {
                    _UserId = value;
                    RaisePropertyChanged(nameof(UserId));
                }
            }
        }
        decimal _Cost;
        public decimal Cost
        {
            get
            {
                return _Cost;
            }
            set
            {
                _Cost = value;
                RaisePropertyChanged("Cost");

            }
        }

        string _Address;
        public string Address
        {
            get
            {
                return _Address;
            }
            set
            {
                _Address = value;
                RaisePropertyChanged("Address");

            }
        }

        DateTime _DateSelected;
        public DateTime DateSelected
        {
            get
            {
                return _DateSelected;
            }
            set
            {
                _DateSelected = value;
                RaisePropertyChanged("DateSelected");

            }
        }


        string _imgname;
        public string imgname
        {
            get
            {
                return _imgname;
            }
            set
            {
                _imgname = value;
                RaisePropertyChanged("imgname");
            }
        }
        ImageSource _PreviewImageSource;
        public ImageSource PreviewImageSource
        {
            get
            {
                return _PreviewImageSource;
            }

            set
            {
                _PreviewImageSource = value;
                RaisePropertyChanged("PreviewImageSource");
            }
        }

        public ProviderModel CurrentProvider { get; set; }
        public ServiceModel Currentservices{ get; set; }
        public users Currentuser { get; set; }
        public RequestModel Request { get; set; }
        public Command PostService { get; set; }
        public Command RatingChangedCommand { get; set; }
        public Command GetLocationCommand { get; }
        public Command GoRate { get; set; }
        public Command imgPic { get; set; }

        public override void Init(object initData)
        {
            base.Init(initData);
            CurrentProvider = initData as ProviderModel;
           // Currentservices = initData as ServiceModel;
            UserId = CurrentProvider.userId;
            Currentuser = initData as users;
        }

        IRequestService _request;
       
        public RequsetViewModel()
        {
            PostService = new Command(OnPostService);
            imgPic = new Command(OnimgPic);
            GoRate = new Command(OnGoToRate);
            GetLocationCommand = new Command(async () => await GetLocationAsync());
        }
        private async Task GetLocationAsync()
        {
            try
            {
                var location = await Geolocation.GetLastKnownLocationAsync();

                if (location != null)
                {
                    var placemarks = await Geocoding.GetPlacemarksAsync(location.Latitude, location.Longitude);
                    var placemark = placemarks?.FirstOrDefault();

                    if (placemark != null)
                    {
                        Address = $"{placemark.Thoroughfare} {placemark.Locality}, {placemark.AdminArea}";
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions
                Console.WriteLine($"Error retrieving location: {ex.Message}");
            }
        }

        ObservableCollection<users> _Listusers;
        public ObservableCollection<users> Listusers
        {

            get
            {
                return _Listusers;
            }
            set
            {
                _Listusers = value;
                RaisePropertyChanged("Listusers");
            }
        }

        RequestModel ORequestModel = new RequestModel();
        async  void OnPostService()
        {
          
            ORequestModel.ServiceId = CurrentProvider.ServiceId;
            ORequestModel.ProviderId = CurrentProvider.ProvicderId;
         ORequestModel.ProviderName = CurrentProvider.ProviderName;
            ORequestModel.ServiceNameAr = CurrentProvider.ServiceNameAr;
           
          ORequestModel.ServiceName = CurrentProvider.ServiceName;
            //ORequestModel.ser = CurrentProvider.ServiceName;
            ORequestModel.Cost = Cost;
           // ORequestModel.CustomerId = Currentuser.CustomerId;

            ORequestModel.Date = DateSelected;
           
         ORequestModel.Descriptions = Description;
           ORequestModel.ImageNameProblem = imgname;
            
            ORequestModel.StatusOfRequest = null;
            ORequestModel.IsResponse = null;
            ORequestModel.Address = Address;

            if (Cost <= 0 || string.IsNullOrEmpty(Description) || string.IsNullOrEmpty(Address) || string.IsNullOrEmpty(DateSelected.ToString()))
            {
                await App.Current.MainPage.Navigation.PushPopupAsync(new Helpers.MsgxfForgetmailPhone(), true);
                return;
            }


            if (ORequestModel!=null)
            {


                // Fetch the list of users from the API
                string json = await Helpers.Utility.CallWebApi("/api/user");
                List<users> Listusers = JsonConvert.DeserializeObject<List<users>>(json);

                // Find the user with matching CustomerName in the Listusers
                users user = Listusers.FirstOrDefault(u => u.CustomerId == UserId);

                if (user != null)
                {
                    // Assign the CustomerId from the found user to ORequestModel.CustomerId
                    ORequestModel.CustomerId = user.CustomerId;
                }
            }

            UserDialogs.Instance.ShowLoading("يتم التحميل");
            string result = await Helpers.Utility.PostData($"/api/Request/{ORequestModel.CustomerId}",
             JsonConvert.SerializeObject(ORequestModel));

            if (result == "\"done\"")
            {
                await App.Current.MainPage.Navigation.PushPopupAsync(new Helpers.Resultsent(), true);
                UserDialogs.Instance.HideLoading();
                return;

            }
            else
            {
                await App.Current.MainPage.DisplayAlert("cds", "error", "ok");
                UserDialogs.Instance.HideLoading();


                return;
            }



        }

        private ImageSource _selectedImage;

        public ImageSource SelectedImage
        {
            get => _selectedImage;
            set
            {
                _selectedImage = value;
                RaisePropertyChanged("SelectedImage");
            }
        }

      

        async void OnimgPic()
        {
            string action = await Application.Current.MainPage.DisplayActionSheet("Pick Image", "Cancel", null, "Camera", "Gallery");

            if (action == "Camera" && !MediaPicker.IsCaptureSupported)
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Camera is not supported on this device.", "OK");
                return;
            }

            try
            {
                MediaPickerOptions options = new MediaPickerOptions
                {
                    Title = "Pick Image"
                };

                FileResult result = null;

                if (action == "Camera")
                {
                    result = await MediaPicker.CapturePhotoAsync(options);
                }
                else if (action == "Gallery")
                {
                    result = await MediaPicker.PickPhotoAsync(options);
                }
                else
                {
                    // User canceled, do nothing
                    return;
                }

                if (result != null)
                {
                    // Get the URL of the selected image
                    string imageUrl = result.FullPath;

                    // Assign the image URL to imgname
                    imgname = imageUrl;

                    // Assign the imgname value to oRequestModel.ImageName
                    ORequestModel.ImageNameProblem = imgname;

                    // Assign the image URL to PreviewImageSource directly
                    PreviewImageSource = ImageSource.FromUri(new Uri(imageUrl));
                    string imagePath = result.FullPath;
                    Stream imageStream = await result.OpenReadAsync();

                    SelectedImage = ImageSource.FromStream(() => imageStream);
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions that might occur during the image selection process.
                await Application.Current.MainPage.DisplayAlert("Error", $"Error: {ex.Message}", "OK");
            }
        }

        //public async void OnimgPic()
        //{
        //    string action = await Application.Current.MainPage.DisplayActionSheet("Pick Image", "Cancel", null, "Camera", "Gallery");

        //    if (action == "Camera" && !MediaPicker.IsCaptureSupported)
        //    {
        //        await Application.Current.MainPage.DisplayAlert("Error", "Camera is not supported on this device.", "OK");
        //        return;
        //    }

        //    try
        //    {
        //        MediaPickerOptions options = new MediaPickerOptions
        //        {
        //            Title = "Pick Image"
        //        };

        //        FileResult result = null;

        //        if (action == "Camera")
        //        {
        //            result = await MediaPicker.CapturePhotoAsync(options);
        //        }
        //        else if (action == "Gallery")
        //        {
        //            result = await MediaPicker.PickPhotoAsync(options);
        //        }
        //        else
        //        {
        //            // User canceled, do nothing
        //            return;
        //        }

        //        if (result != null)
        //        {
        //            // Get the URL of the selected image
        //            string imageUrl = result.FullPath;

        //            // Assign the image URL to imgname
        //            imgname = imageUrl;

        //            // Assign the image URL to PreviewImageSource directly
        //            PreviewImageSource = ImageSource.FromUri(new Uri(imageUrl));
        //            string imagePath = result.FullPath;
        //            Stream imageStream = await result.OpenReadAsync();

        //            SelectedImage = ImageSource.FromStream(() => imageStream);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        // Handle any exceptions that might occur during the image selection process.
        //        await Application.Current.MainPage.DisplayAlert("Error", $"Error: {ex.Message}", "OK");
        //    }
        //}

        //async Task<string> UploadImageToServer(string localImagePath)
        //{
        //    // Call your server API to upload the image and get the public URL
        //    // Example:
        //    // string publicImageUrl = await YourServerApi.UploadImage(localImagePath);

        //    // For now, return a placeholder URL
        //    return "http://ashello-001-site1.ktempurl.com/images/your_image.jpg";
        //}

        //async void OnPostService()
        //{
        //    ORequestModel.ServiceId = CurrentProvider.ServiceId;
        //    ORequestModel.ProviderId = CurrentProvider.ProvicderId;
        //    ORequestModel.ProviderName = CurrentProvider.ProviderName;

        //    ORequestModel.ServiceName = CurrentProvider.ServiceNameAr;
        //    ORequestModel.Cost = Cost;
        //    ORequestModel.Date = DateSelected;

        //    if (!string.IsNullOrEmpty(imgname))
        //    {
        //        // Upload the image to the server and get the public URL
        //        string publicImageUrl = await UploadImageToServer(imgname);

        //        // Update the ORequestModel.ImageNameProblem with the public URL
        //        ORequestModel.ImageNameProblem = publicImageUrl;
        //    }

        //    ORequestModel.Descriptions = Description;
        //    ORequestModel.Address = Address;

        //    if (Cost <= 0 || string.IsNullOrEmpty(Description) || string.IsNullOrEmpty(Address) || string.IsNullOrEmpty(DateSelected.ToString()))
        //    {
        //        await App.Current.MainPage.Navigation.PushPopupAsync(new Helpers.MsgxfForgetmailPhone(), true);
        //        return;
        //    }

        //    if (ORequestModel != null)
        //    {
        //        // Fetch the list of users from the API
        //        string json = await Helpers.Utility.CallWebApi("/api/user");
        //        List<users> Listusers = JsonConvert.DeserializeObject<List<users>>(json);

        //        // Find the user with matching CustomerName in the Listusers
        //        users user = Listusers.FirstOrDefault(u => u.CustomerId == UserId);

        //        if (user != null)
        //        {
        //            // Assign the CustomerId from the found user to ORequestModel.CustomerId
        //            ORequestModel.CustomerId = user.CustomerId;
        //        }
        //    }

        //    UserDialogs.Instance.ShowLoading("يتم التحميل");
        //    string result = await Helpers.Utility.PostData($"/api/Request/{ORequestModel.CustomerId}", JsonConvert.SerializeObject(ORequestModel));

        //    if (result == "\"done\"")
        //    {
        //        await App.Current.MainPage.Navigation.PushPopupAsync(new Helpers.Resultsent(), true);
        //    }
        //    else
        //    {
        //        await App.Current.MainPage.DisplayAlert("cds", "error", "ok");
        //    }

        //    UserDialogs.Instance.HideLoading();
        //}




        async void OnGoToRate()
        {
            var rateViewModel = new otp.ViewModels.RateViewModel(CurrentProvider); // Instantiate the RateViewModel with parameters
            var ratePage = new otp.Pages.RatePage { BindingContext = rateViewModel }; // Instantiate the RatePage and set its BindingContext to the RateViewModel
            await PopupNavigation.Instance.PushAsync(ratePage); // Push the RatePage as a popup
        }





    }
}
