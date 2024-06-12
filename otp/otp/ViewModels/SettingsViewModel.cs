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
  public  class SettingsViewModel : FreshBasePageModel
    {
      
        public string Title { get; set; } = "SettingPage";
      

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
      
     
      
        public Command ChangePassword { get; set; }
        public Command imgPic { get; set; }






        protected override void ViewIsAppearing(object sender, EventArgs e)
        {
            base.ViewIsAppearing(sender, e);

            // Deserialize the userinfo string back to a list of users
          
        }


        public SettingsViewModel()
        {
           
            NavigateEnglish = new Command(OnNavigateEnglish);
            NavigateArabic = new Command(OnNavigateArabic);


            ChangePassword = new Command(OnChangePassword);
         
          
            Listusers = new ObservableCollection<users>();

        

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
         

          
            
            string customerName = initData.ToString();

            if (!string.IsNullOrEmpty(customerName))
            {
                string json1 = await Helpers.Utility.CallWebApi("/api/user");
                List<users> Listusers = JsonConvert.DeserializeObject<List<users>>(json1);

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


                base.Init(initData);
        }
        
        void OnNavigateEnglish()
        {
            CultureInfo cal;
            cal = new CultureInfo("en");
            Resources.AppResources.Culture = cal;
            Helpers.Settings.Language = "en";
            CoreMethods.PushPageModel<HomeViewModel>(Username,modal:true);
          


        }

        void OnNavigateArabic()
        {

           


            CultureInfo cal;
            cal = new CultureInfo("ar");
           Resources.AppResources.Culture = cal;
            Helpers.Settings.Language = "ar";
            CoreMethods.PushPageModel<HomeViewModel>(Username, modal: true);
            

        }
      async  void OnChangePassword()
        {
          await  CoreMethods.PushPageModel<ResetPasswordViewModel>(null, modal: true);
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
      
     

      
       








      

       async public void Onlogout()
        {
             DependencyService.Get<IAuthenticationService>().SignOut();
            await CoreMethods.PushPageModel < LoginViewModel >();
        }

      


        async void OnRemoveFromCart(RequestModel ORequestModel)
        {
            string result = await Helpers.Utility.DeleteData($"{Helpers.Utility.ServerUrDatabase}/api/Request", ORequestModel.ID);
            await App.Current.MainPage.Navigation.PushPopupAsync(new Helpers.MsgDelete(), true);

        }
      

        async void OnGoChat()
        {
            await CoreMethods.PushPageModel<ChatViewModel>();
        }


    }
}
