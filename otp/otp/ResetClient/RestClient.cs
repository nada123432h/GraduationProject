using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace otp.ResetClient
{
   
        public class RestClient<T>
        {
        


        private const string MainWebServiceUrl = "http://192.168.159.89:8012/api/UserCredentials/username=ali@gmail.com/password=123";
            private const string LoginWebServiceUrl = "http://testingme333-001-site1.etempurl.com/api/UserCredentials/";

            public async Task<bool> checkLogin(string username, string password)
            {
            string url = $"http://192.168.159.89:8012/api/UserCredentials/username={username}/password={password}";
            var httpClient = new HttpClient();

                var response = await httpClient.GetAsync(MainWebServiceUrl );

                return response.IsSuccessStatusCode;
            }
        
    }
}
