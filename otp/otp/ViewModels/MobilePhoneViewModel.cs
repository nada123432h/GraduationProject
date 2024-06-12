using Acr.UserDialogs;
using FreshMvvm;
using Newtonsoft.Json;
using otp.Models;
using Rg.Plugins.Popup.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace otp.ViewModels
{
    public class MobilePhoneViewModel : FreshBasePageModel
    {
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

        private string _code;
        public string Code
        {
            get => _code;
            set
            {
                if (_code != value)
                {
                    _code = value;
                    RaisePropertyChanged(nameof(Code));
                }
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

        private bool _codeSent;
        public bool CodeSent
        {
            get => _codeSent;
            set
            {
                if (_codeSent != value)
                {
                    _codeSent = value;
                    RaisePropertyChanged(nameof(CodeSent));
                }
            }
        }

        private string _buttonText = "ارسال الرمز";
        public string ButtonText
        {
            get => _buttonText;
            set
            {
                if (_buttonText != value)
                {
                    _buttonText = value;
                    RaisePropertyChanged(nameof(ButtonText));
                }
            }
        }
        private string _verificationId;
        public string VerificationId
        {
            get => _verificationId;
            set
            {
                if (_verificationId != value)
                {
                    _verificationId = value;
                    RaisePropertyChanged(nameof(VerificationId));
                }
            }
        }

        public Command NextCommand { get; set; }


        IAuthenticationService auth;
        // private INavigationService _navigationService;
        public bool _codeRequested;
        
        public MobilePhoneViewModel()

        {

           
            auth = DependencyService.Get<IAuthenticationService>();

            // _navigationService = navigationService;

            NextCommand = new Command(OnNextAction);

        }
        users Ouser = new users();

        string phonePattern = @"^01[0-2]{1}[0-9]{8}$";

        private async void OnNextAction()
        {

            
            if (string.IsNullOrEmpty(PhoneNumber) || string.IsNullOrEmpty(Username))
            {
                await App.Current.MainPage.Navigation.PushPopupAsync(new Helpers.MsgxfForgetmailPhone(), true);
                return;
            }
            if (!Regex.IsMatch(PhoneNumber, phonePattern))
            {
                // Invalid email format
                await App.Current.MainPage.Navigation.PushPopupAsync(new Helpers.ErrorOnphone(), true);
                return;
            }

            string json1 = await Helpers.Utility.CallWebApi("/api/user");
            List<users> Listusers = JsonConvert.DeserializeObject<List<users>>(json1);

            // Deserialize the existing user info from settings if available
            List<users> userList = !string.IsNullOrEmpty(Helpers.Settings.userinfo) ?
                                   JsonConvert.DeserializeObject<List<users>>(Helpers.Settings.userinfo) :
                                   new List<users>();
            if (userList == null)
            {
                userList = new List<users>();
            }
          
            
            bool usernameExists = userList.Any(u => u.CustomerName == Username);
            bool phoneNumberExists = userList.Any(u => u.CustomerPhone == PhoneNumber);

            if (usernameExists && phoneNumberExists)
            {
                ButtonText = "الدخول";
                UserDialogs.Instance.ShowLoading("يتم التحميل");
               await CoreMethods.PopPageModel();
                await CoreMethods.PushPageModel<FingerPrintViewModel>(Username, true);

                UserDialogs.Instance.HideLoading();
                return;
            }
                if (_codeRequested)
            {

                // verify code that user entered.
                var loginAttempt = await auth.VerifyOtpCodeAsync(Code);
                if (loginAttempt)
                {
                    UserDialogs.Instance.ShowLoading("يتم التحميل");

                    if (userList == null)
                    {
                        userList = new List<users>();
                    }
                    users newUser = new users
                    {

                        CustomerName = Username,
                        CustomerPhone = PhoneNumber
                    };

                    // Add the new user object to the list
                    // userList.Clear();
                  userList.Add(newUser);
                    //
                    // Serialize the updated list back to a JSON string
                    string updatedUserInfo = JsonConvert.SerializeObject(userList);

                    // Store the updated list back to settings
                    Helpers.Settings.userinfo = updatedUserInfo;

                    Ouser.CustomerPhone = PhoneNumber;
                    Ouser.CustomerName = Username;
                    
                    string result = await Helpers.Utility.PostData("/api/user",
                    JsonConvert.SerializeObject(Ouser));
                 // string userinfo = JsonConvert.SerializeObject(Ouser);

                    await CoreMethods.PushPageModel<FingerPrintViewModel>(Username,true);
                    UserDialogs.Instance.HideLoading();

                }
                else
                {
                    await App.Current.MainPage.Navigation.PushPopupAsync(new Helpers.ErrorOnphone(), true);
                    return;


                }
            }
            else
            {
                CodeSent = await auth.AuthenticationMobile(PhoneNumber);

                if (!CodeSent)
                {
                     await App.Current.MainPage.Navigation.PushPopupAsync(new Helpers.ErrorOnphone(), true);
                    return;
                }
                await App.Current.MainPage.Navigation.PushPopupAsync(new Helpers.MessageBox(), true);


                _codeRequested = true;
                ButtonText = "تأكيد الرمز";


            }
        }
       
    }
}
