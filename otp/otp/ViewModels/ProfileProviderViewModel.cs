using FreshMvvm;
using Newtonsoft.Json;
using otp.Helpers;
using otp.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace otp.ViewModels
{
  
   public class ProfileProviderViewModel : FreshBasePageModel
    {

        public ProviderModel CurrentOfProvider { get; set; }
        public string Title { get; set; } = "في الخدمه";
        public Command ChangePassword { get; set; }
        public Command imgPic { get; set; }
        public Command NavigateEnglish { get; set; }
        public Command logout { get; set; }

        public Command NavigateArabic { get; set; }
        public async override void Init(object initData)
        {
            base.Init(initData);
          
            CurrentOfProvider = initData as ProviderModel;
           
        }
        string _txtemail;
        public string txtemail
        {
            get
            {
                return _txtemail;
            }
            set
            {
                _txtemail = value;
                RaisePropertyChanged("txtemail");

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
        public event PropertyChangedEventHandler RatingChanged;

        private double _selectedRating;
    public double SelectedRating
    {
        get => _selectedRating;
        set
        {
            if (_selectedRating != value)
            {
                _selectedRating = value;
                RaisePropertyChanged(nameof(SelectedRating));
                    RatingChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedRating)));
                }
        }
    }
        public Command RatingChangedCommand { get; set; }
        public ProfileProviderViewModel()
        {
           txtemail=Helpers.Settings.ProviderEmail;
            imgPic = new Command(OnimgPic);
            logout = new Command(Onlogout);
            NavigateEnglish = new Command(OnNavigateEnglish);


            RatingChangedCommand = new Command(async () => await UpdateRatingInDatabase());

            NavigateArabic = new Command(OnNavigateArabic);
            ChangePassword = new Command(OnChangePassword);
        }
      
        void OnNavigateEnglish()
        {
            CultureInfo cal;
            cal = new CultureInfo("en");
            Resources.AppResources.Culture = cal;
            Helpers.Settings.Language = "en";
            CoreMethods.PushPageModel<LoginProviderViewModel>(null, true);
            //CoreMethods.PushPageModel<LoginViewModel>();



        }

        void OnNavigateArabic()
        {




            CultureInfo cal;
            cal = new CultureInfo("ar");
            Resources.AppResources.Culture = cal;
            Helpers.Settings.Language = "ar";
            CoreMethods.PushPageModel<LoginProviderViewModel>(null, true);


        }
        async void OnChangePassword()
        {
            await CoreMethods.PushPageModel<ResetPasswordViewModel>(null, true);
        }
        async public void Onlogout()
        {
            DependencyService.Get<IAuthenticationService>().SignOut();
            await CoreMethods.PushPageModel<LoginProviderViewModel>(null, true);
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
                    Title = "تغيير الصوره الشخصيه"
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
                    //imgname = imageUrl;
                    otp.Helpers.Settings.ImgName = imageUrl;
                    // Assign the imgname value to oRequestModel.ImageName
                    // ORequestModel.ImageName = imgname;

                    // Assign the image URL to PreviewImageSource directly
                    //PreviewImageSource = ImageSource.FromUri(new Uri(imageUrl));
                    //   string imagePath = result.FullPath;
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
        

    

      

        private async Task UpdateRatingInDatabase()
        {
            string result = await Utility.UpdateRating(CurrentOfProvider.ProvicderId, SelectedRating);
        }

    }
}
