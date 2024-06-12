using FreshMvvm;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace otp.ViewModels
{
   public class EmptyListViewModel: FreshBasePageModel
    {
        public Command GoBackCommand { get; set; }
     
        async void OnGoBackCommand()
        {
            await CoreMethods.PopPageModel(true);
        }

        public EmptyListViewModel()
        {
            GoBackCommand = new Command(OnGoBackCommand);
        }


    }
}
