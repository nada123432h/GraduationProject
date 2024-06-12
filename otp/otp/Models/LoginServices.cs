using otp.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using otp.RestAPIClient;

namespace otp.Models
{
    public class LoginServices 
    {
        RestClient<UserInfo> _restClient = new RestClient<UserInfo>();
        public async Task<bool> CheckLoginIfExists(string username, string password)
        {
            var check = await _restClient.checkLogin(username, password);

            return check;
        }

        //public async Task<UserInfo> Login(string UserName, string Password)
        //{
        //    var userInfo = new List<UserInfo>();
        //    var client = new HttpClient();
        //    //   string url = $"http://172.22.208.1:8012/api/Login/userName={UserName}&Password={Password}";
        //    string url = "/api/UserCredentials/username=ali@gmail.com/password=123";


        //   // client.BaseAddress = new Uri(url);
        //   // HttpResponseMessage response = await client.GetAsync("");
        //    //if (response.IsSuccessStatusCode)
        //    //{
        //       // string content = response.Content.ReadAsStringAsync().Result;
        //        //userInfo = JsonConvert.DeserializeObject<List<UserInfo>>(content);


        //        string json = await Helpers.Utility.CallWebApi(url);
        //        userInfo = JsonConvert.DeserializeObject<List<UserInfo>>(json);
        //    return userInfo.FirstOrDefault();
        //       // return await Task.FromResult(userInfo.FirstOrDefault());
        //    //}
        //    //else
        //    //{
        //    //    return null;
        //    //}

        //}

        public class ApiResponse
        {
            public bool Successful => ErrorMessage == null;
            public string ErrorMessage { get; set; }
            public string Response { get; set; }
        }
    }
}
