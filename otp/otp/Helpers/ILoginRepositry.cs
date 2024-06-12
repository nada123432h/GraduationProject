using otp.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace otp.Helpers
{
    public interface ILoginRepositry
    {

        Task<UserInfo> Login(string UserName, string Password);
    }
}
