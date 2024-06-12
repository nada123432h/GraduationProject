using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace otp.Models
{
    public class GoogleUser
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public Uri Picture { get; set; }
    }
    public interface IGoogleManager
    {
        Task Login(Action<GoogleUser, string> OnLoginComplete);

        void Logout();
    }
}
