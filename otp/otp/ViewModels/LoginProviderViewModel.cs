using Acr.UserDialogs;
using FreshMvvm;
using Newtonsoft.Json;
using otp.Models;
using Rg.Plugins.Popup.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace otp.ViewModels
{
    public class LoginProviderViewModel : FreshBasePageModel
    {
        // readonly ILoginRepositry _LoginRepositry = new LoginServices();



        public LoginProviderViewModel()
        {
            NavigateToHome = new Command(OnNavigateToHome);
          

            auth = DependencyService.Get<IAuthenticationService>();
            GoToForget = new Command(OnGoToForget); 



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
        public ProviderModel CurrentProvider { get; set; }

        public Command NavigateToHome { get; set; }
        public Command GoToForget { get; set; }
        public Command GoRegistet { get; set; }
       // public ProviderModel CurrentProvider { get; set; }


        IAuthenticationService auth;
        Provider oProvider = new Provider();

        public async void OnNavigateToHome(object obj)
        {
            var pop2 = new Helpers.MsgxPass();
            if (string.IsNullOrEmpty(txtPass) || string.IsNullOrEmpty(txtemail))
            {
                await App.Current.MainPage.Navigation.PushPopupAsync(pop2, true);
                return;
            }
            try
            {


                Helpers.Settings.ProviderEmail = txtemail;
                bool token = await auth.SignIn(txtemail, txtPass);
                if (!token)
                {

                    await App.Current.MainPage.Navigation.PushPopupAsync(pop2, true);
                    return;
                }
                if (txtemail.EndsWith("@gmail.com", StringComparison.OrdinalIgnoreCase))
                {
                    await App.Current.MainPage.DisplayAlert("خطأ", "بريدك الإلكتروني لا يمكن أن ينتهي بـ .gmail.com", "حسناً");
                    return;
                }

                else
                {
                    UserDialogs.Instance.ShowLoading("يتم التحميل");
                    string json = await Helpers.Utility.CallWebApi($"/api/Provider");
                    List<Provider> providerList = JsonConvert.DeserializeObject<List<Provider>>(json);

                    // Extract the first part of the email before '@'
                    string[] emailParts = txtemail.Split('@');
                    string emailName = emailParts[0];

                    // Find the provider with matching name
                    Provider matchedProvider = providerList.FirstOrDefault(provider => provider.ProviderName == emailName);

                    if (matchedProvider != null)
                    {
                        // You have found the matched provider
                        int providerId = matchedProvider.ProvicderId;
                        // Now you can use the providerId as needed
                    }

                   
                    await CoreMethods.PushPageModel<RequsetProviderViewModel>(matchedProvider,true);

                    UserDialogs.Instance.HideLoading();
                }
                  

               

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
            await CoreMethods.PushPageModel<ForgetPasswordViewModel>(null, true);

        }




    }
}

