using Acr.UserDialogs;
using CustomApp.Android;
using Firebase;
using Firebase.Auth;
using Java.Util.Concurrent;
using otp;
using Rg.Plugins.Popup.Extensions;
using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xamarin.Essentials;

[assembly: Xamarin.Forms.Dependency(typeof(FirebaseAuthentication))]

namespace CustomApp.Android
{
    public class FirebaseAuthentication : PhoneAuthProvider.OnVerificationStateChangedCallbacks, IAuthenticationService

    {

        const int OTP_TIMEOUT = 80; // seconds
        private TaskCompletionSource<bool> _phoneAuthTcs;
        private string _verificationId;

        public async Task<bool> CreateUser(string username, string email, string password)
        {
            try
            {
                var authResult = await FirebaseAuth.Instance
                     .CreateUserWithEmailAndPasswordAsync(email, password);

                var userProfileChangeRequestBuilder = new UserProfileChangeRequest.Builder();
                userProfileChangeRequestBuilder.SetDisplayName(username);

                var userProfileChangeRequest = userProfileChangeRequestBuilder.Build();
                await authResult.User.UpdateProfileAsync(userProfileChangeRequest);
                return await Task.FromResult(true);
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("The email address is already in use by another account"))

                {
                    var pop2 = new otp.Helpers.MsgxExist();
                    await App.Current.MainPage.Navigation.PushPopupAsync(pop2, true);

                }


                Console.WriteLine(ex.Message);
                return await Task.FromResult(false);

            }


        }


        public bool IsSignIn() => FirebaseAuth.Instance.CurrentUser != null;






        public async Task<bool> ResetPassword(string email)
        {
            try
            {
                // Attempt to send password reset email
                await FirebaseAuth.Instance.SendPasswordResetEmailAsync(email);
                return true; // Email sent successfully
            }
            catch (Firebase.Auth.FirebaseAuthInvalidUserException ex)
            {
                // Handle case where email doesn't exist
                Console.WriteLine($"Email not found: {email}");
                return false; // Email not found
            }
            catch (Exception ex)
            {
                // Handle other exceptions here
                Console.WriteLine($"Exception: {ex.Message}");
                return false;
            }
        }



        public async Task<bool> SignIn(string email, string password)
        {
            try
            {
                email = email.Replace(" ", string.Empty);
                UserDialogs.Instance.ShowLoading("يتم التحميل");
                var authResult = await FirebaseAuth.Instance.SignInWithEmailAndPasswordAsync(email, password);
                return authResult != null; // Return true if sign-in is successful
            }
            catch (FirebaseAuthInvalidUserException ex)
            {
                Console.WriteLine($"Invalid user: {ex.Message}");
            }
            catch (FirebaseAuthInvalidCredentialsException ex)
            {
                Console.WriteLine($"Invalid credentials: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
            }
            UserDialogs.Instance.HideLoading();

            return await Task.FromResult(false);
        }




        public void SignOut()
        {
            try
            {
                FirebaseAuth.Instance.SignOut();
            }
            catch (FirebaseException ex)
            {
                Console.WriteLine($"Firebase sign-out error: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception during sign-out: {ex.Message}");
            }
        }

        private TaskCompletionSource<bool> _verificationCodeCompletionSource;
        private string _verificationID;
        public async Task<bool> AuthenticationMobile(string phoneNumber)
        {
            _phoneAuthTcs = new TaskCompletionSource<bool>();

            try
            {
                var currentActivity = Xamarin.Essentials.Platform.CurrentActivity;

                if (currentActivity == null)
                {
                    // Handle the case where the current activity is not available
                    _phoneAuthTcs.TrySetResult(false);
                }
                else
                {
                    // Ensure the phoneNumber includes the country code for Egypt (+20)
                    string fullPhoneNumber = "+2" + phoneNumber;
                    if (!IsValidPhoneNumberFormat(fullPhoneNumber))
                    {
                        // Handle the case where the phone number is not valid
                        _phoneAuthTcs.TrySetResult(false);
                        return false;
                    }
                    _verificationCodeCompletionSource = new TaskCompletionSource<bool>();
                    var authOption = PhoneAuthOptions.NewBuilder()
                         .SetPhoneNumber(fullPhoneNumber)
                         .SetTimeout((Java.Lang.Long)60L, Java.Util.Concurrent.TimeUnit.Seconds)
                         .SetActivity(Platform.CurrentActivity)
                         .SetCallbacks(this).Build();
                    PhoneAuthProvider.VerifyPhoneNumber(authOption);
                    // return await _verificationCodeCompletionSource.Task;
                    return true;


                }
            }
            catch (FirebaseAuthInvalidCredentialsException ex)
            {
                // Handle invalid credentials (e.g., invalid phone number)
                Console.WriteLine($"Firebase authentication error: {ex.Message}");
                _phoneAuthTcs.TrySetResult(false);
            }
            catch (FirebaseException ex)
            {
                // Handle other Firebase exceptions
                Console.WriteLine($"Firebase exception: {ex.Message}");
                _phoneAuthTcs.TrySetResult(false);
            }
            catch (Exception ex)
            {
                // Handle other generic exceptions
                Console.WriteLine($"Exception: {ex.Message}");
                _phoneAuthTcs.TrySetResult(false);
            }

            return true;
        }


        private bool IsValidPhoneNumberFormat(string phoneNumber)
        {
            // Regular expression pattern to match Egyptian phone numbers
            string pattern = @"^\+20\d{10}$";

            // Check if the phone number matches the pattern
            return Regex.IsMatch(phoneNumber, pattern);
        }


        public override void OnVerificationFailed(FirebaseException p0)
        {
            _phoneAuthTcs.SetResult(false);
        }


        public override void OnVerificationCompleted(PhoneAuthCredential p0)
        {
            System.Diagnostics.Debug.WriteLine("PhoneAuthCredential created Automatically");

        }





        public override void OnCodeSent(string verificationID, PhoneAuthProvider.ForceResendingToken p1)
        {
            base.OnCodeSent(verificationID, p1);
            _verificationCodeCompletionSource.SetResult(true);
            _verificationID = verificationID;
        }


        //public void SignOut() => FirebaseAuth.Instance.SignOut();
        private void OnAuthCompleted(Task task, TaskCompletionSource<bool> tcs)
        {
            if (task.IsCanceled || task.IsFaulted)
            {
                // something went wrong
                tcs.SetResult(false);
                return;
            }
            _verificationId = null;
            tcs.SetResult(true);
        }
        public async Task<bool> VerifyOtpCodeAsync(string code)
        {

            bool returnValue = false;
            if (!string.IsNullOrWhiteSpace(_verificationID))
            {
                var credential = PhoneAuthProvider.GetCredential(_verificationID, code);

                await FirebaseAuth.Instance.SignInWithCredentialAsync(credential).ContinueWith((authTask) =>
                {
                    if (authTask.IsFaulted || authTask.IsCanceled)
                    {
                        returnValue = false;
                        return;
                    }
                    returnValue = true;

                });
            }
            return returnValue;
        }

        public async Task<bool> Replace(string email, string pass, string newpass)
        {
            try
            {
                var res = await FirebaseAuth.Instance.SignInWithEmailAndPasswordAsync(email, pass);


               await res.User.UpdatePasswordAsync(newpass);
                return true;
            }
            catch (FirebaseAuthInvalidCredentialsException)
            {
                return false;
            }

            catch (Exception ex)
            {
                // Other exceptions
                return false;
            }
        }
    }

}
