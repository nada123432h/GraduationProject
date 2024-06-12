using FreshMvvm;
using Plugin.Fingerprint;
using Rg.Plugins.Popup.Extensions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace otp.ViewModels
{
    public class FingerPrintViewModel : FreshBasePageModel
    {


        public Command GoFingerPrint { get; set; }
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
        public Command GoHomePage { get; set; }
        public FingerPrintViewModel()
        {
            GoFingerPrint = new Command(OnGoFingerPrint);
            GoHomePage = new Command(OnGoHomePage);
        }
       async void OnGoFingerPrint()
        {
            var availability = await CrossFingerprint.Current.IsAvailableAsync();
            var pop2= new Helpers.FingerDone();
            if (!availability)
            {
                await App.Current.MainPage.DisplayAlert("information", "no biomertics avaliable", "ok");
                return;
            }
            var authResult = await CrossFingerprint.Current.AuthenticateAsync(new
                Plugin.Fingerprint.Abstractions.AuthenticationRequestConfiguration("Heads up !", "I want to use your biomatrics "));
            if (authResult.Authenticated)
            {
                await App.Current.MainPage.Navigation.PushPopupAsync(pop2, true);
               
                await CoreMethods.PushPageModel<HomeViewModel>(UserName, true);
            }

        }
        public override void Init(object initData)
        {
            string customerName = initData.ToString();
            UserName = customerName;

            base.Init(initData);
        }
        async  void OnGoHomePage()
        {
            await CoreMethods.PushPageModel<HomeViewModel>(UserName,true);

        }

    }
}

