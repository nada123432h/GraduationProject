
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
   public class ForgetPasswordViewModel: FreshBasePageModel
    {
        UserRepository _userRepository = new UserRepository();

        public Command SendCode { get; set; }


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
        IAuthenticationService auth;


        public ForgetPasswordViewModel()
        {
            auth = DependencyService.Get<IAuthenticationService>();

            SendCode = new Command(OnSendCode);
        }
        string emailPattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";

        async void OnSendCode()
        {
            var user = await auth.ResetPassword(txtemail);
            if (string.IsNullOrEmpty(txtemail))
            {
                var pop2 = new Helpers.MsgxfForgetEmail();
                await App.Current.MainPage.Navigation.PushPopupAsync(pop2, true);
               
            }
            else
            {

         
            try
                {
                    if (!user)
                    {
                        if (!Regex.IsMatch(txtemail, emailPattern))
                        {
                            // Invalid email format
                            await App.Current.MainPage.Navigation.PushPopupAsync(new Helpers.MsgxFormat(), true);
                            return;
                        }
                    }


                    UserDialogs.Instance.ShowLoading("يتم التحميل");
                    //await App.Current.MainPage.Navigation.PushPopupAsync(new Helpers.MsgxfForgetEmail(), true);
                    await auth.ResetPassword(txtemail);
                    await App.Current.MainPage.Navigation.PushPopupAsync(new Helpers.MessageBox(), true);
                   // await CoreMethods.PushPageModel<LoginViewModel>();
                    UserDialogs.Instance.HideLoading();
                }
           
           
            catch (Exception ex)
            {
                if (txtemail != null)
                {
                    

                    return;
                }
                Console.WriteLine(ex.Message);

                await Xamarin.Forms.Shell.Current
                    .DisplayAlert("Password Reset", "An error occurs", "OK");
            }

        }
        }
    }
    }

