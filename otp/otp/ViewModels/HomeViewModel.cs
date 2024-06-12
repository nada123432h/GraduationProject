using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using otp.Models;
using FreshMvvm;

using Xamarin.Forms;
using Xamarin.Essentials;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Plugin.StoreReview;
using otp.Helpers;
using Rg.Plugins.Popup.Extensions;

namespace otp.ViewModels
{
  public  class HomeViewModel:FreshBasePageModel
    {
      
        public string Title { get; set; } = "في الخدمه";
      

        ObservableCollection<ServiceModel> _ListServicesl;
        public ObservableCollection<ServiceModel> ListServices {

            get
            {
                return _ListServicesl;
            } 
            set
            {
                _ListServicesl = value;
                RaisePropertyChanged("ListServices");
            } 
        }
        bool isRefreshing;
        public bool IsRefreshing
        {
            get { return isRefreshing; }
            set
            {
                if (isRefreshing != value)
                {
                    isRefreshing = value;
                    RaisePropertyChanged(nameof(IsRefreshing));
                }
            }
        }

        ObservableCollection<ServiceModel> _ListFilterServicesl;
        public ObservableCollection<ServiceModel> ListFilterServicesl
        {

            get
            {
                return _ListFilterServicesl;
            }
            set
            {
                _ListFilterServicesl = value;
                RaisePropertyChanged("ListFilterServicesl");
            }
        }
        private bool _isLoading;
        public bool IsLoading
        {
            get { return _isLoading; }
            set
            {
                if (_isLoading != value)
                {
                    _isLoading = value;
                    RaisePropertyChanged("IsLoading");
                }
            }
        }

        private bool _IsLoadingServices;
        public bool IsLoadingServices
        {
            get { return _IsLoadingServices; }
            set
            {
                if (_IsLoadingServices != value)
                {
                    _IsLoadingServices = value;
                    RaisePropertyChanged("IsLoadingServices");
                }
            }
        }

        ObservableCollection<Models.RequestModel> _lstRequests;
        public ObservableCollection<Models.RequestModel> lstRequests
        {
            get
            {
                return _lstRequests;
            }
            set
            {

                _lstRequests = value;
                RaisePropertyChanged("lstRequests");
            }
        }
        public Command NavigateEnglish { get; set; }
        
     
        public Command NavigateArabic { get; set; }
        public Command SelectService { get; set; }
        public Command SearchWithText { get; set; }
        public Command OnRefreshRequested { get; }
        public Command logout { get; set; }
        public Command ChangePassword { get; set; }
        public Command imgPic { get; set; }
        public Command GoChat { get; set; }
        public Command GoToRequest { get; set; }



        public Command RemoveFromCart { get; set; }
        public Command Confirmed { get; set; }
        public Command GoSettings { get; set; }



        public HomeViewModel()
        {
           
           


            ChangePassword = new Command(OnChangePassword);
            imgPic = new Command(OnimgPic);
            RemoveFromCart = new Command<RequestModel>(OnRemoveFromCart);
            Listusers = new ObservableCollection<users>();


            SearchWithText = new Command(OnSearchWithText);
            GoToRequest = new Command(OnGoToRequest);
            GoChat = new Command(OnGoChat);
            GoSettings = new Command(OnGoSettings);

            SelectService = new Command<ServiceModel>(OnSelectService);
            ListServices = new ObservableCollection<ServiceModel>();
            
            foreach (var service in ListServices)
            {
                service.ServiceName = (Helpers.Settings.Language == "ar") ? service.ServiceNameAr : service.ServiceName;

            }

            OnRefreshRequested = new Command(async () => await RefreshCategories());
            lstRequests = new ObservableCollection<RequestModel>();

          

           

            logout = new Command(Onlogout);

           

         Confirmed = new Command<RequestModel>(OnConfirmed);

        }
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


        async void OnSearchWithText()
        {

            if (string.IsNullOrEmpty(SearchTerm))
            {
                ListFilterServicesl = new ObservableCollection<ServiceModel>(ListServices);
                // lstFiltermodel2 = new ObservableCollection<itemPhoneModel>(lstmodel2);
                return;
            }
            if (Helpers.Settings.Language == "ar")
            {
                ListFilterServicesl = new ObservableCollection<ServiceModel>(ListServices.Where(a => a.ServiceNameAr.Contains(SearchTerm)));
            }
            else
            {
                ListFilterServicesl = new ObservableCollection<ServiceModel>(ListServices.Where(a => a.ServiceName.Contains(SearchTerm)));

            }

            //lstFiltermodel2 = new ObservableCollection<itemPhoneModel>(lstFiltermodel2.Where(a => a.ItemName.Contains(SearchTerm)));

        }
      async  void OnSelectService(ServiceModel service)
        {
            string json1 = await Helpers.Utility.CallWebApi("/api/user");

            List<users> Listusers = JsonConvert.DeserializeObject<List<users>>(json1);

            // Deserialize the userinfo string back to a list of users
            string userinfoString = Helpers.Settings.userinfo;
            var userList = JsonConvert.DeserializeObject<List<users>>(userinfoString);
            Listusers.Reverse();

            // Find the user in Listusers that matches the name from userList


           

            if (!string.IsNullOrEmpty(Username))
            {
                // Check if the customer name exists in the userList
                var user = Listusers.FirstOrDefault(u => u.CustomerName == Username);
                if (user != null)
                {
                    // Found user with the matching customer name
                   
                    service.userId = user.CustomerId; // Assuming customerId is needed further
                                                      // Additional logic using customerId can be added here if needed
                }
            }



          await  CoreMethods.PushPageModel<ServiceMapViewModel>(service,modal:true);
        }
        private string _phoneNumber;
        public string PhoneNumber
        {
            get => _phoneNumber;
            set
            {
                if (_phoneNumber != value)
                {
                    _phoneNumber = value;
                    RaisePropertyChanged(nameof(PhoneNumber));
                }
            }
        }
        public string _UserEmail;

        public string UserEmail
        {
            get
            {
                return _UserEmail;
            }
            set
            {
                _UserEmail = value;
                RaisePropertyChanged("UserEmail");
            }
        }
        private string _Username;
        public string Username
        {
            get => _Username;
            set
            {
                if (_Username != value)
                {
                    _Username = value;
                    RaisePropertyChanged(nameof(Username));
                }
            }
        }

        public async override void Init(object initData)
        {
            IsLoading = true;
            IsLoadingServices = false;

            try
            {

               
                var _service = new Services.ServicesService();
               
                ListServices = await _service.GetServices();
                ListFilterServicesl = new ObservableCollection<ServiceModel>(ListServices);


                foreach (var service in ListServices)
                {
                    // service.ServiceName = (Helpers.Settings.Language == "ar") ? service.ServiceNameAr : service.ServiceName;
                    if (Helpers.Settings.Language == "en")
                    {
                        service.ServiceName = service.ServiceName;
                        
                        CultureInfo cal;
                        cal = new CultureInfo("en");
                        Resources.AppResources.Culture = cal;


                    }
                    else
                    {
                        service.ServiceName = service.ServiceNameAr;
                        CultureInfo cal;
                        cal = new CultureInfo("ar");
                        Resources.AppResources.Culture = cal;
                    }
                }
            }
            finally
            {
                IsLoadingServices = true;
                IsLoading = false;
            }
            string json1 = await Helpers.Utility.CallWebApi("/api/user");
            List<users> Listusers = JsonConvert.DeserializeObject<List<users>>(json1);
 
            // Deserialize the userinfo string back to a list of users

            string customerName = initData.ToString();
           
           


            if (!string.IsNullOrEmpty(customerName))
            {
                // Check if the customer name exists in the userList
                var user = Listusers.FirstOrDefault(u => u.CustomerName == customerName);
                if (user != null)
                {
                    // Found user with the matching customer name
                    Username = user.CustomerName;
                    PhoneNumber = user.CustomerPhone;
                    UserEmail = user.CustomerEmail;

                    int customerId = user.CustomerId; // Assuming customerId is needed further
                                                      // Additional logic using customerId can be added here if needed
                }
            }
            string userinfoString = Helpers.Settings.userinfo;
            var userList = JsonConvert.DeserializeObject<List<users>>(userinfoString);

            if (userList != null && userList.Any())
            {
                // Find the user whose username matches the current user
                var currentUser = userList.FirstOrDefault(u => u.CustomerName == Username);

                if (currentUser != null)
                {
                    // Set the SelectedImage property to the image specified by SelectedImageUrl
                    SelectedImage = currentUser.SelectedImageUrl;
                   
                }
            }


            base.Init(initData);
        }
       

        void OnNavigateArabic()
        {

           


            CultureInfo cal;
            cal = new CultureInfo("ar");
           Resources.AppResources.Culture = cal;
            Helpers.Settings.Language = "ar";
            CoreMethods.PushPageModel<HomeViewModel>(Username,true);
            

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
        async  void OnChangePassword()
        {
          await  CoreMethods.PushPageModel<ResetPasswordViewModel>(null,true);
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
                    string imageUrl = result.FullPath;
                    imgname = imageUrl;

                    string userinfoString = Helpers.Settings.userinfo;
                    var userList = JsonConvert.DeserializeObject<List<users>>(userinfoString);
                    if (userList != null && userList.Any())
                    {
                        // Find the user to update
                        users userToUpdate = userList.FirstOrDefault(u => u.CustomerName == Username);
                        if (userToUpdate != null)
                        {
                            userToUpdate.SelectedImageUrl = imgname;
                            SelectedImage = ImageSource.FromUri(new Uri(imageUrl));
                            string imagePath = result.FullPath;
                            // Update the SelectedImageUrl property of the user
                            

                            // Set SelectedImage to the image source
                            Stream imageStream = await result.OpenReadAsync();
                            SelectedImage = ImageSource.FromStream(() => imageStream);
                        }
                    }

                    // Serialize the updated userList back to JSON
                    string updatedUserInfo = JsonConvert.SerializeObject(userList);
                    // Store the updated userList back to settings
                    Helpers.Settings.userinfo = updatedUserInfo;
                }

            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"Error: {ex.Message}", "OK");
            }
        }

        protected override void ViewIsAppearing(object sender, EventArgs e)
        {
            base.ViewIsAppearing(sender, e);

            // Deserialize the userinfo string back to a list of users
            string userinfoString = Helpers.Settings.userinfo;
            var userList = JsonConvert.DeserializeObject<List<users>>(userinfoString);

            if (userList != null && userList.Any())
            {
                // Find the user whose username matches the current user
                var currentUser = userList.FirstOrDefault(u => u.CustomerName == Username);

                if (currentUser != null)
                {
                    // Set the SelectedImage property to the image specified by SelectedImageUrl
                    SelectedImage = currentUser.SelectedImageUrl;
                }
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
         bool _isConfirmed;

        public bool IsConfirmed
        {
            get { return _isConfirmed; }
            set
            {
                if (_isConfirmed != value)
                {
                    _isConfirmed = value;
                    RaisePropertyChanged(nameof(IsConfirmed));
                }
            }
        }

        string _StatusLabel;
        public string StatusLabel
        {
            get { return _StatusLabel; }
            set
            {
                if (_StatusLabel != value)
                {
                    _StatusLabel = value;
                    RaisePropertyChanged("StatusLabel"); // Notify the UI that the property value has changed
                }
            }
        }
        async Task<ObservableCollection<RequestModel>> GetCategories()
        {
            try
            {
                // Deserialize the JSON string from the API call to get the list of users
                string json1 = await Helpers.Utility.CallWebApi("/api/user");
                List<users> Listusers = JsonConvert.DeserializeObject<List<users>>(json1);
               
                // Deserialize the userinfo string back to a list of users
                string userinfoString = Helpers.Settings.userinfo;
                var userList = JsonConvert.DeserializeObject<List<users>>(userinfoString);
                Listusers.Reverse();
               
             
               
                users userFromApi = null;
                int customerId;
                var user = Listusers.FirstOrDefault(u => u.CustomerName == Username);
                if (user != null)
                {
                    // Found user with the matching customer name
                    Username = user.CustomerName;
                    PhoneNumber = user.CustomerPhone;
                    userFromApi = Listusers.FirstOrDefault(u => u.CustomerName == Username && u.CustomerPhone == PhoneNumber);
                    if (userFromApi != null)
                    {
                        customerId = userFromApi.CustomerId;
                    }
                    customerId = user.CustomerId;
                }
                

                if (userinfoString != null)
                {
                    // Access the CustomerId property of the found user
                   

                    // Now you can use customerId as needed

                    string json = await Helpers.Utility.CallWebApi($"/api/Request/{userFromApi.CustomerId}");

                    ObservableCollection<RequestModel> lstRequests = JsonConvert.DeserializeObject<ObservableCollection<RequestModel>>(json);
                    if (lstRequests.Count == 0)
                        await CoreMethods.PushPageModel<EmptyListViewModel>(null,modal:true);
                    // Assuming lstRequests contains RequestModel objects with a property named ImageUrl
                    foreach (var request in lstRequests)
                    {
                        // Create an Image control for each image URL
                        Image image = new Image();

                        // Check if the image URL is a local file path
                        if (Uri.TryCreate(request.ImageNameProblem, UriKind.Absolute, out Uri uri) && uri.IsFile)
                        {
                            // Set the image source to a file image source
                            image.Source = ImageSource.FromFile(uri.LocalPath);
                        }
                        if (request.StatusOfRequest == true)
                        {
                            request.statusLabel = "تم قبول طلبك";
                            request.IsConfirmed = true;
                            request.StatusColor = Color.FromHex("#65B741");
                            request.IsButtonEnabled = false;
                            request.isVisableButton = false;
                            request.IsWaiting = false;
                        }
                        else if (request.StatusOfRequest == null)
                        {
                            request.statusLabel = ".. طلبك قيد الانتظار";
                            request.StatusColor = Color.FromHex("#0766AD");
                            request.IsConfirmed = false;
                            request.isVisableButton = true;
                            request.IsWaiting = true;
                            request.IsButtonEnabled = false;
                        }
                        else if (request.StatusOfRequest == false)
                        {
                            request.statusLabel = "..تم تعديل طلبك ";
                            request.StatusColor = Color.FromHex("#952323");
                            request.IsConfirmed = false;
                            request.isVisableButton = true;
                            request.IsButtonEnabled = true;

                            request.IsWaiting = false;


                        }

                        // Add the image control to your layout (e.g., a StackLayout)
                        // For example:
                        // stackLayout.Children.Add(image);
                    }

                    IsLoading = true;
                    IsRefreshing = false;
                    return lstRequests;
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions that might occur during the process.
                await Application.Current.MainPage.DisplayAlert("Error", $"Error: {ex.Message}", "OK");
            }

            return null;
        }








        async Task RefreshCategories()
        {
            IsRefreshing = true;

            // Simulate API call
            await Task.Delay(2000);

            // Populate Requests collection with dummy data
            lstRequests.Clear();

            lstRequests = await GetCategories();
            //  RaisePropertyChanged(nameof(lstRequests));
            foreach (var request in lstRequests)
            {
                // Create an Image control for each image URL
                Image image = new Image();

               
                if (request.IsResponse == true)
                {
                    request.statusLabel = "سيتم وصول مزود خدمتك ف الميعاد";
                    request.StatusColor = Color.FromHex("#65B741");
                    request.IsConfirmed = true;
                    request.isVisableButton = false;
                    request.IsButtonEnabled = false;

                    request.IsWaiting = false;
                }
            }

                IsLoading = true;
            IsRefreshing = false;
        }

       async public void Onlogout()
        {
             DependencyService.Get<IAuthenticationService>().SignOut();
            await CoreMethods.PushPageModel < LoginViewModel >(Username,modal:true);
        }

        async void OnConfirmed(RequestModel oRequestModel)
        {
            int id = oRequestModel.ID; // Extract the ID of the request
            oRequestModel.IsResponse = true; // Update the status to confirmed in the local model

            // Call the UpdateStatusOfRequest method from Utility to update the status on the server
            string result = await Utility.UpdateStatusOfResponse(id,oRequestModel.StatusOfRequest,oRequestModel.IsResponse,oRequestModel.Cost,oRequestModel.Date);
            foreach (var request in lstRequests)
            {
                if (request.StatusOfRequest == false)
                {
                    request.statusLabel = "..تم تعديل طلبك ";
                    request.StatusColor = Color.FromHex("#952323");
                    request.IsConfirmed = true;
                    request.isVisableButton = false;
                    request.IsButtonEnabled = false;

                    request.IsWaiting = false;
                }
            }
            oRequestModel.IsButtonEnabled = false;
          oRequestModel.isVisableButton = false;
            await App.Current.MainPage.Navigation.PushPopupAsync(new Helpers.MsgSendToProvider(), true);
           

        }


        async void OnRemoveFromCart(RequestModel ORequestModel)
        {
            string result = await Helpers.Utility.DeleteData($"{Helpers.Utility.ServerUrDatabase}/api/Request", ORequestModel.ID);
            await App.Current.MainPage.Navigation.PushPopupAsync(new Helpers.MsgDelete(), true);

        }
        async void OnGoToRequest()
        {
           await GetCategories();
        }
        async void OnGoSettings()
        {
            await CoreMethods.PushPageModel<SettingsViewModel>(Username,modal:true);

        }

        async void OnGoChat()
        {
            await CoreMethods.PushPageModel<ChatViewModel>(null,true);
        }


    }
}
