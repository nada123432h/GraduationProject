
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace otp
{
    public class UserRepository
    {
       public string webAPIKey = "AIzaSyD-AIzaSyBUnc0LxLH2kuZtbgV9oizWu87FjLY6Jyo";
        //FirebaseAuthProvider authProvider;
        
        //public UserRepository()
        //{
        //   authProvider = new FirebaseAuthProvider(new FirebaseConfig(webAPIKey));
        //}

        //public async Task<bool> Register(string email,string name,string password)
        //{

        //    var token = await authProvider.CreateUserWithEmailAndPasswordAsync(email, password, name);
        //    if (!string.IsNullOrEmpty(token.FirebaseToken))
        //    {
        //        return true;
        //    }
        //    return false;
        //}

        //public async Task<string>SignIn(string email,string password)
        //{
        //    var token = await authProvider.SignInWithEmailAndPasswordAsync(email, password);            
        //    if(!string.IsNullOrEmpty(token.FirebaseToken))
        //    {
        //        return token.FirebaseToken;
        //    }
        //    return "";
        //}

       // public async Task<bool> ResetPassword(string email)
        //{
            //try
            //{
            //    await authProvider.SendPasswordResetEmailAsync(email);
            //    return true;
            //}
            //catch (Exception ex)
            //{
            //    // Log the exception message or details
            //    Console.WriteLine($"Error sending password reset email: {ex.Message} - {ex.Message}");
            //    return false;
            //}
       // }


    }
}
