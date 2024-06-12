using FreshMvvm;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace otp.ViewModels
{
   public class ChoseViewModel: FreshBasePageModel
    {

        public Command GoLogin { get; set; }
        public Command GoLoginProvider { get; set; }

        public ChoseViewModel()
        {
            GoLogin = new Command(OnGoLogin);
            GoLoginProvider = new Command(OnGoLoginProvider);
        }

       async  void OnGoLogin()
        {
            await CoreMethods.PushPageModel<LoginViewModel>(null,true);
        }
        async void OnGoLoginProvider()
        {
            await CoreMethods.PushPageModel<IntroductionViewModel>(null, modal:true);
        }
    }
}
