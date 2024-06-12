using FreshMvvm;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace otp.ViewModels
{
  public  class ConnectionViewModel: FreshBasePageModel
    {
        public Command GoBack { get; set; }
        public ConnectionViewModel()
        {
            GoBack = new Command(OnGoBackCommand);
        }

        async void OnGoBackCommand()
        {
            await CoreMethods.PopPageModel();
        }

    }
}
