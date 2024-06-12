using System;
using System.Threading.Tasks;

namespace otp
{
    public interface IAuthenticationService
    {
        bool IsSignIn();
        Task<bool> CreateUser(string username, string email, string password);
        void SignOut();
        Task<bool> SignIn(string email, string password);
        Task<bool> VerifyOtpCodeAsync(string code);
        Task<bool> AuthenticationMobile(string phoneNumber);
        Task<bool> ResetPassword(string email);
        Task<bool> Replace(string email, string pass, string newpass);
       // Task<bool> IsEmailRegisteredAsync(string email);
    }
}
