using Acr.UserDialogs;
using FreshMvvm;
using Rg.Plugins.Popup.Extensions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using Xamarin.Forms;

namespace otp.ViewModels
{
    class ResetPasswordViewModel: FreshBasePageModel
    {
        public ResetPasswordViewModel()
        {
            auth = DependencyService.Get<IAuthenticationService>();
            NextCommand = new Command(OnNextCommand);

        }
        public Command NextCommand { get; set; }
        string emailPattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";


        string _Password;
        public string Password
        {
            get
            {
                return _Password;
            }
            set
            {
                _Password = value;
                RaisePropertyChanged("Password");

            }
        }

        string _Email;
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
        string _newPassword;
        public string newPassword
        {
            get
            {
                return _newPassword;
            }
            set
            {
                _newPassword = value;
                RaisePropertyChanged("newPassword");

            }
        }

        IAuthenticationService auth;
        async void OnNextCommand()
        {

            
            bool token = await auth.Replace(Email, Password,newPassword);
            if (token)
            {

                UserDialogs.Instance.ShowLoading("يتم التحميل");
                await App.Current.MainPage.Navigation.PushPopupAsync(new Helpers.MsgDoneChange(), true);
                UserDialogs.Instance.HideLoading();
                return;
            }
            else
            {
                if(string.IsNullOrEmpty(Email)|| !Regex.IsMatch(Email, emailPattern)|| string.IsNullOrEmpty(Password)|| string.IsNullOrEmpty(newPassword))
                {
                    await App.Current.MainPage.Navigation.PushPopupAsync(new Helpers.MsgxPass(), true);
                    return;
                }
                
            }
        }
    }
}
