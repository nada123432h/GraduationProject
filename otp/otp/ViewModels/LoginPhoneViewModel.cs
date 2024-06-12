using FreshMvvm;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace otp.ViewModels
{
    public class LoginPhoneViewModel : FreshBasePageModel
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

        private string _buttonText = "Send Code";
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

        public LoginPhoneViewModel()

        {
            auth = DependencyService.Get<IAuthenticationService>();

            // _navigationService = navigationService;

            NextCommand = new Command(OnNextAction);

        }

        private async void OnNextAction()
        {
            if (_codeRequested)
            {
                // verify code that user entered.
                var loginAttempt = await auth.VerifyOtpCodeAsync(Code);
                if (loginAttempt)
                {
                    Helpers.Settings.Phone = PhoneNumber;
                    await CoreMethods.PushPageModel<FingerPrintViewModel>(null,true);

                }
                else
                {
                    // Something went wrong
                    // TODO: Alert via Dialog Service.
                }
            }
            else
            {
                CodeSent = await auth.AuthenticationMobile(PhoneNumber);

                if (!CodeSent)
                    return;

                _codeRequested = true;
                ButtonText = "Verify Code";
            }
        }
    }
}
