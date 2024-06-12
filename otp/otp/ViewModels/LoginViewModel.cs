using otp.Helpers;
using otp.Models;

using FreshMvvm;
using Newtonsoft.Json;
using Rg.Plugins.Popup.Extensions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Acr.UserDialogs;

namespace otp.ViewModels
{
     public class LoginViewModel: FreshBasePageModel
    {
      // readonly ILoginRepositry _LoginRepositry = new LoginServices();


      
        public LoginViewModel()
        {
            NavigateToHome = new Command(OnNavigateToHome);
            GoRegistet = new Command(OnGoRegistet);
            GoToForget = new Command(OnGoToForget);
            GoPhone = new Command(OnGoPhone);
            auth = DependencyService.Get<IAuthenticationService>();

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

        string _txtPass;
        public string txtPass
        {
            get
            {
                return _txtPass;
            }
            set
            {
                _txtPass = value;
                RaisePropertyChanged("txtPass");

            }
        }
  
        private INavigation _navigation;

        public Command NavigateToHome { get; set; }
        public Command GoToForget { get; set; }
        public Command GoPhone { get; set; }
        public Command GoRegistet { get; set; }
        public string webApiKey = "AIzaSyBUnc0LxLH2kuZtbgV9oizWu87FjLY6Jyo";
        IAuthenticationService auth;

        public async void OnNavigateToHome(object obj)
        {
           
            var pop2 = new Helpers.MsgxPass();
            if (string.IsNullOrEmpty(txtPass) || string.IsNullOrEmpty(txtemail))
            {
                await App.Current.MainPage.Navigation.PushPopupAsync(pop2, true);
                return;
            }
            if (txtemail.EndsWith("@org.com", StringComparison.OrdinalIgnoreCase))
            {
                await App.Current.MainPage.DisplayAlert("خطأ", "بريدك الإلكتروني لا يمكن أن ينتهي بـ @org.com", "حسناً");
                return;
            }
            try
            {


                bool token = await auth.SignIn(txtemail, txtPass);
                string username = txtemail.Substring(0, txtemail.IndexOf('@'));

                if (!token)
                {

                    await App.Current.MainPage.Navigation.PushPopupAsync(pop2, true);
                    return;
                }
                else
                    Helpers.Settings.Username = txtemail;
              

                await CoreMethods.PushPageModel<FingerPrintViewModel>(username,true);
                UserDialogs.Instance.HideLoading();
                return;
            }
           
            catch (Exception ex)
            {
                // Handle other exceptions
                await App.Current.MainPage.Navigation.PushPopupAsync(pop2, true);
                return;
                // Optionally display an error message or log the exception
            }
        }

        async void OnGoToForget()
        {
            UserDialogs.Instance.ShowLoading("يتم التحميل");
            await CoreMethods.PushPageModel<ForgetPasswordViewModel>(null,true);
            UserDialogs.Instance.HideLoading();
        }
        async public void OnGoPhone()
        {
            await CoreMethods.PushPageModel<MobilePhoneViewModel>(null, true);

        }
        async public void OnGoRegistet()
        {

            await CoreMethods.PushPageModel<SignUpViewModel>(null,true);

        }


    }
}
