
using otp.Models;
using FreshMvvm;
using Rg.Plugins.Popup.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using System.Linq;
using Acr.UserDialogs;

namespace otp.ViewModels
{
    public class SignUpViewModel : FreshBasePageModel
    {
        public string webApiKey = "AIzaSyBUnc0LxLH2kuZtbgV9oizWu87FjLY6Jyo";

        private INavigation _navigation;


        public event PropertyChangedEventHandler PropertyChanged;

        public string _Email;
        public string Email
        {
            get
            {
                return _Email;
            }
            set
            {
                _Email = value;
                RaisePropertyChanged("Email");
            }
        }

        public string _passwordl;

        public string Password
        {
            get
            {
                return _passwordl;
            }
            set
            {
                _passwordl = value;
                RaisePropertyChanged("Password");
            }
        }

        public string _UserPhone;

        public string UserPhone
        {
            get
            {
                return _UserPhone;
            }
            set
            {
                _UserPhone = value;
                RaisePropertyChanged("UserPhone");
            }
        }
      
        public string _UserName;

        public string UserName
        {
            get
            {
                return _UserName;
            }
            set
            {
                _UserName = value;
                RaisePropertyChanged("UserName");
            }
        }

        public Command RegisterUser { get; }
        public Command GoGmail { get; set; }
        public Command GoLogout { get; set; }
     
       

        private void RaisePropertyChanged(string v)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(v));
        }

        IAuthenticationService auth;
        public  IGoogleManager _googleManager;
        GoogleUser GoogleUser = new GoogleUser();
        public bool IsLogedIn { get; set; }
        public SignUpViewModel()
        {
          
         
            auth = DependencyService.Get<IAuthenticationService>();
            RegisterUser = new Command(RegisterUserTappedAsync);
           
            GoGmail = new Command(() => Login());
           
            GoLogout = new Command(OnGoLogout);
            

        }
      
        private void OnGoLogout()
        {
            _googleManager = DependencyService.Get<IGoogleManager>();


            _googleManager.Logout();
        }




        async public Task Login()
        {
             _googleManager = DependencyService.Get<IGoogleManager>();
          await  _googleManager.Login(OnLoginComplete);
        }
        public async void OnLoginComplete(GoogleUser googleUser, string message)
        {
            if (googleUser != null)
            {

                UserDialogs.Instance.ShowLoading("يتم التحميل");
                string email = googleUser.Email;
                int atIndex = email.IndexOf('@');

                string json1 = await Helpers.Utility.CallWebApi("/api/user");
                List<users> Listusers = JsonConvert.DeserializeObject<List<users>>(json1);

                // Deserialize the existing user info from settings if available
                List<users> userList = !string.IsNullOrEmpty(Helpers.Settings.userinfo) ?
                                       JsonConvert.DeserializeObject<List<users>>(Helpers.Settings.userinfo) :
                                       new List<users>();
               // userList.Clear();

                bool usernameExists = Listusers.Any(u => u.CustomerName == email.Substring(0, atIndex));

                if (atIndex != -1)
                {
                    string username = email.Substring(0, atIndex);

                    users newUser = new users
                    {
                        CustomerName = username,
                        CustomerEmail = email
                    };

                    if (usernameExists)
                    {
                        await CoreMethods.PushPageModel<HomeViewModel>(username, true);
                        return;
                    }

                    // Add the new user object to the list
                   // userList.Clear();
                    userList.Add(newUser);

                    // Serialize the updated list back to a JSON string
                    string updatedUserInfo = JsonConvert.SerializeObject(userList);

                    // Store the updated list back to settings
                    Helpers.Settings.userinfo = updatedUserInfo;

                    Ouser.CustomerEmail = email;
                    Ouser.CustomerName = username;
                    string result = await Helpers.Utility.PostData("/api/user", JsonConvert.SerializeObject(Ouser));

                    // Now you can save the username or use it as needed
                    // For example, save it to your settings
                    Helpers.Settings.Username = email;

                    // Navigate to the HomeViewModel
                    await CoreMethods.PushPageModel<HomeViewModel>(username, true);
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Message", message, "Ok");
                }
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Message", message, "Ok");
            }
        }


        users Ouser = new users();
        private async void RegisterUserTappedAsync()
        {
         
            var pop2 = new Helpers.MsgxPass();
            if (string.IsNullOrEmpty(Password) || string.IsNullOrEmpty(Email)|| string.IsNullOrEmpty(UserPhone)|| string.IsNullOrEmpty(UserName))
            {
                await App.Current.MainPage.Navigation.PushPopupAsync(new Helpers.MsgxfForgetmailPhone(), true);
                return;

                
            }
           
        
            string emailPattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
            string phonePattern = @"^01[0-2]{1}[0-9]{8}$";

            var user = await auth.CreateUser(UserName,Email, Password);
            if (!Regex.IsMatch(UserPhone, phonePattern))
            {
                // Invalid email format
                await App.Current.MainPage.Navigation.PushPopupAsync(new Helpers.ErrorOnphone(), true);
                return;
            }


            if (!user)
            {
                if (Password.Length < 6)
                {
                    await App.Current.MainPage.Navigation.PushPopupAsync(new Helpers.MsgxPassLength(), true);
                    return;
                }
                if (!Regex.IsMatch(Email, emailPattern))
                {
                    // Invalid email format
                    await App.Current.MainPage.Navigation.PushPopupAsync(pop2, true);
                    return;
                }
                
                await App.Current.MainPage.Navigation.PushPopupAsync(pop2, true);

                return;
            }
            if (user)
            {

                UserDialogs.Instance.ShowLoading("يتم التحميل");
                List<users> userList = JsonConvert.DeserializeObject<List<users>>(Helpers.Settings.userinfo);
                userList.Clear();
                if (userList == null)
                {
                    userList = new List<users>();
                }
              
                string[] emailParts = Email.Split('@');
                string emailUsername = emailParts[0];

                if (emailUsername != UserName)
                {
                    // Username doesn't match the part before '@' in the email address
                    await App.Current.MainPage.Navigation.PushPopupAsync(new Helpers.MsgxUsernameNotMatch(), true);
                    return;
                }
                // Create a new user object
                users newUser = new users
                {

                    CustomerName = UserName,
                    CustomerPhone = UserPhone,
                    CustomerEmail = Email
                };

                // Add the new user object to the list
              //  userList.Clear();
                userList.Add(newUser);
                //
                // Serialize the updated list back to a JSON string
                string updatedUserInfo = JsonConvert.SerializeObject(userList);
                UserDialogs.Instance.HideLoading();
                // Store the updated list back to settings
                Helpers.Settings.userinfo = updatedUserInfo;

                Ouser.CustomerPhone = UserPhone;
                Ouser.CustomerName = UserName;
                Ouser.CustomerEmail = Email;
                string result = await Helpers.Utility.PostData("/api/user",
                JsonConvert.SerializeObject(Ouser));
                string userinfo = JsonConvert.SerializeObject(Ouser);
            
                Helpers.Settings.Phone = Ouser.CustomerPhone;

                await App.Current.MainPage.Navigation.PushPopupAsync(new Helpers.RegisterationDone(), true);
                await CoreMethods.PushPageModel<LoginViewModel>(null, true);

                return;
            }

          
           

        }
    }


}
