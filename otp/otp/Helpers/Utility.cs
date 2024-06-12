
using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using System.Net.Http;
using System.Net;
using System.Net.Http.Headers;
using Xamarin.Forms;
using Newtonsoft.Json;
using otp.Models;
using System.Text;

namespace otp.Helpers
{
    public class ClsApiParamters
    {
        public string ParamterName { get; set; }
        public string ParamterValue { get; set; }
    }

    public static class Utility
    {
        static bool InWork = false;
        //#if DEBUG
        //        public static readonly string ServerUrl = "https://www.dt-works.com/amd/horse";
        //#else
        //        //public static readonly string ServerUrl = "https://www.psaiahf.com/horse";
        //#endif
        public static readonly string ServerUrl = "http://172.22.208.1:8012/";
        public static readonly string ServerUrl1 = "http://92.205.28.199:4178/";
        public static readonly string ServerUrDatabase = "http://ashello-001-site1.ktempurl.com/";
        public static string StripHTML(string input)
        {
            return Regex.Replace(input, "<.*?>", String.Empty);
        }

        public static string FormatTextWithoutnewLin(string input)
        {
            return input.Replace("\r", " ").Replace("\n", ".");
        }


        /// <summary>
        /// Converts unix date to C# date time
        /// </summary>
        /// <param name="UnixDate"></param>
        /// <returns></returns>
        public static DateTime ConvertUnixToDateTime(string UnixDate)
        {

            if (string.IsNullOrEmpty(UnixDate))
                return new DateTime();

            // Example of a UNIX timestamp for 11-29-2013 4:58:30
            double timestamp = double.Parse(UnixDate);

            // Format our new DateTime object to start at the UNIX Epoch
            System.DateTime dateTime = new System.DateTime(1970, 1, 1, 0, 0, 0, 0);

            // Add the timestamp (number of seconds since the Epoch) to be converted
            dateTime = dateTime.AddSeconds(timestamp);

            return dateTime;

        }


        /// <summary>
        /// Return the HTTP client for the service address 
        /// </summary>
        /// <returns>HTTP Client Object </returns>
        public static HttpClient GetClient()
        {
            CookieContainer cookieContainer = new CookieContainer();
            Cookie cookie = new Cookie();
            Uri baseAddress = new Uri(Utility.ServerUrDatabase);
            var handler = new HttpClientHandler() { CookieContainer = cookieContainer };
            var client = new HttpClient(handler) { BaseAddress = baseAddress };

            if (cookie != null && !string.IsNullOrWhiteSpace(cookie.Value))
            {
                cookieContainer.Add(baseAddress, cookie);
            }

            return client;
        }


        public static async Task<string> CallWebApi(string url)
        {

            //if (InWork)
            //    return "inwork";

            InWork = true;

            try
            {
                using (var client = GetClient())
                {
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    //client.DefaultRequestHeaders.Add("authorization", "Basic YmtkcmR0NGl0OjZKTDJuRXlGNW04WWg=");

                    try
                    {

                        HttpResponseMessage loginResponse = await client.GetAsync(Utility.ServerUrDatabase + url);
                        if (loginResponse.IsSuccessStatusCode)
                        {
                            using (var responseContent = loginResponse.Content)
                            {
                                var result = responseContent.ReadAsStringAsync().Result;
                                return result;
                            }
                        }

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        throw;
                    }
                }
            }
            catch (WebException ex)
            {
                //await App.Current.MainPage.DisplayAlert("message from server", sResult, "");
                //Log.Report(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                //await App.Current.MainPage.DisplayAlert("message from server", sResult, "");
                //Log.Report(ex);
            }
            finally
            {
                InWork = false;
            }
            return null;
        }


        public static async Task<string> CallWebApi(ClsApiParamters Pramater, string url)
        {
            InWork = true;

            var parameters = new List<KeyValuePair<string, string>>();
            parameters.Add(new KeyValuePair<string, string>(Pramater.ParamterName, Pramater.ParamterValue));

            HttpContent content = new FormUrlEncodedContent(parameters);
            try
            {
                using (var client = GetClient())
                {
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    //client.DefaultRequestHeaders.Add("authorization", "Basic YmtkcmR0NGl0OjZKTDJuRXlGNW04WWg=");
                    try
                    {
                        string URL = (url[0] == 'h') ? url : Utility.ServerUrl + url;
                        HttpResponseMessage loginResponse = await client.GetAsync(URL);
                        if (loginResponse.IsSuccessStatusCode)
                        {
                            using (var responseContent = loginResponse.Content)
                            {
                                var result = responseContent.ReadAsStringAsync().Result;
                                return result;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        return "WebException";
                    }
                }
            }
            catch (WebException ex)
            {
                //await App.Current.MainPage.DisplayAlert("message from server", sResult, "");
                //Log.Report(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                //await App.Current.MainPage.DisplayAlert("message from server", sResult, "");
                //Log.Report(ex);
            }
            finally
            {
                InWork = false;
            }
            return null;
        }

        public static async Task<string> PostData(string url, string content)
        {
            var httpLient = new HttpClient();
            httpLient.DefaultRequestHeaders.Accept.Clear();
            httpLient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            try
            {
                HttpContent Mycontent = new StringContent(content);
                MediaTypeHeaderValue mValue = new MediaTypeHeaderValue("application/json");
                Mycontent.Headers.ContentType = mValue;
                var response = await httpLient.PostAsync(ServerUrDatabase + url, Mycontent);

                // Check if the response was successful
                if (response.IsSuccessStatusCode)
                {
                    var responseData = await response.Content.ReadAsStringAsync();
                    return responseData.ToString();



                }
                else
                {
                    return "api not responding";
                }
            }
            catch (HttpRequestException ex)
            {
                // An error occurred while sending the request
                return "Error sending request: " + ex.Message;
            }
            catch (TaskCanceledException ex)
            {
                // The request timed out
                return "Request timed out: " + ex.Message;
            }
            catch (InvalidOperationException ex)
            {
                // Error while reading response content
                return "Error reading response content: " + ex.Message;
            }
            catch (Exception ex)
            {
                // Other exceptions
                return "Error: " + ex.Message;
            }
        }
        public static async Task<string> DeleteData(string url, int id)
        {
            try
            {
                using (var client = GetClient())
                {
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    HttpResponseMessage response = await client.DeleteAsync($"{url}/{id}");
                    if (response.IsSuccessStatusCode)
                    {
                        string responseData = await response.Content.ReadAsStringAsync();
                        return responseData;
                    }
                    else
                    {
                        return "API not responding";
                    }
                }
            }
            catch (HttpRequestException ex)
            {
                return "Error sending request: " + ex.Message;
            }
            catch (TaskCanceledException ex)
            {
                return "Request timed out: " + ex.Message;
            }
            catch (InvalidOperationException ex)
            {
                return "Error reading response content: " + ex.Message;
            }
            catch (Exception ex)
            {
                return "Error: " + ex.Message;
            }
        }
        public static async Task<string> UpdateStatusOfRequest(int id, bool newStatus, decimal cost, DateTime date)
        {
            try
            {
                // Construct the URL for the GET request to retrieve the JSON response
                string getUrl = $"http://ashello-001-site1.ktempurl.com/api/RequestProvider/GetRequestsById/{id}";

                // Make a GET request to retrieve the JSON response
                using (var client = GetClient())
                {
                    HttpResponseMessage response = await client.GetAsync(getUrl);
                    if (response.IsSuccessStatusCode)
                    {
                        // Read the JSON response
                        string responseData = await response.Content.ReadAsStringAsync();

                        // Deserialize the JSON response to a list of RequestProviderModel objects
                        var requestProviderModels = JsonConvert.DeserializeObject<List<RequestProviderModel>>(responseData);

                        // Check if any objects were returned
                        if (requestProviderModels != null && requestProviderModels.Any())
                        {
                            // Extract the ID from the first object in the list
                            int extractedId = requestProviderModels[0].ID;

                            // Prepare the content to be sent in the request body
                            var content = new StringContent(JsonConvert.SerializeObject(new
                            {
                                StatusOfRequest = newStatus,
                                Cost = cost,
                                Date = date
                            }));
                            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                            // Construct the URL for the PUT request to update the status
                            string putUrl = $"http://ashello-001-site1.ktempurl.com/api/RequestProvider/Put/{extractedId}";

                            // Make a PUT request to update the StatusOfRequest
                            HttpResponseMessage putResponse = await client.PutAsync(putUrl, content);
                            if (putResponse.IsSuccessStatusCode)
                            {
                                string putResponseData = await putResponse.Content.ReadAsStringAsync();
                                return putResponseData;
                            }
                            else
                            {
                                return "PUT request failed";
                            }
                        }
                        else
                        {
                            return "No data found in the response";
                        }
                    }
                    else
                    {
                        return "GET request failed";
                    }
                }
            }
            catch (Exception ex)
            {
                return $"Error: {ex.Message}";
            }
        }

        public static async Task<string> UpdateRating(int providerId, double newRating)
        {
            try
            {
                // Construct the URL for the PUT request to update the rating
                string apiUrl = $"http://ashello-001-site1.ktempurl.com/api/ServiceProviders/Put/{providerId}";

                // Prepare the request body
                var requestBody = new { Rating = newRating };
                var json = JsonConvert.SerializeObject(requestBody);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                // Create HttpClient instance
                using (var httpClient = new HttpClient())
                {
                    // Send PUT request
                    HttpResponseMessage response = await httpClient.PutAsync(apiUrl, content);

                    // Check if the response is successful
                    if (response.IsSuccessStatusCode)
                    {
                        // Read the response content
                        string responseContent = await response.Content.ReadAsStringAsync();
                        return responseContent;
                    }
                    else
                    {
                        // If request fails, return appropriate message
                        return $"Request failed with status code {response.StatusCode}";
                    }
                }
            }
            catch (Exception ex)
            {
                // If an exception occurs, return error message
                return $"Error: {ex.Message}";
            }
        }


        public static async Task<string> UpdateStatusOfResponse(int id, bool? newStatus, bool? newResponse, decimal cost, DateTime date)
        {
            try
            {
                // Construct the URL for the GET request to retrieve the JSON response
                string getUrl = $"http://ashello-001-site1.ktempurl.com/api/RequestProvider/GetRequestsById/{id}";

                // Make a GET request to retrieve the JSON response
                using (var client = GetClient())
                {
                    HttpResponseMessage response = await client.GetAsync(getUrl);
                    if (response.IsSuccessStatusCode)
                    {
                        // Read the JSON response
                        string responseData = await response.Content.ReadAsStringAsync();

                        // Deserialize the JSON response to a list of RequestProviderModel objects
                        var requestProviderModels = JsonConvert.DeserializeObject<List<RequestProviderModel>>(responseData);

                        // Check if any objects were returned
                        if (requestProviderModels != null && requestProviderModels.Any())
                        {
                            // Extract the ID from the first object in the list
                            int extractedId = requestProviderModels[0].ID;

                            // Prepare the content to be sent in the request body
                            var content = new StringContent(JsonConvert.SerializeObject(new
                            {
                                StatusOfRequest = newStatus,
                                IsResponse = newResponse,
                                Cost = cost,
                                Date = date
                            }));
                            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                            // Construct the URL for the PUT request to update the status
                            string putUrl = $"http://ashello-001-site1.ktempurl.com/api/RequestProvider/Put/{extractedId}";

                            // Make a PUT request to update the StatusOfRequest
                            HttpResponseMessage putResponse = await client.PutAsync(putUrl, content);
                            if (putResponse.IsSuccessStatusCode)
                            {
                                string putResponseData = await putResponse.Content.ReadAsStringAsync();
                                return putResponseData;
                            }
                            else
                            {
                                return "PUT request failed";
                            }
                        }
                        else
                        {
                            return "No data found in the response";
                        }
                    }
                    else
                    {
                        return "GET request failed";
                    }
                }
            }
            catch (Exception ex)
            {
                return $"Error: {ex.Message}";
            }
        }




        public static byte[] ReadToEnd(System.IO.Stream stream)
        {
            long originalPosition = 0;

            if (stream.CanSeek)
            {
                originalPosition = stream.Position;
                stream.Position = 0;
            }

            try
            {
                byte[] readBuffer = new byte[4096];

                int totalBytesRead = 0;
                int bytesRead;

                while ((bytesRead = stream.Read(readBuffer, totalBytesRead, readBuffer.Length - totalBytesRead)) > 0)
                {
                    totalBytesRead += bytesRead;

                    if (totalBytesRead == readBuffer.Length)
                    {
                        int nextByte = stream.ReadByte();
                        if (nextByte != -1)
                        {
                            byte[] temp = new byte[readBuffer.Length * 2];
                            Buffer.BlockCopy(readBuffer, 0, temp, 0, readBuffer.Length);
                            Buffer.SetByte(temp, totalBytesRead, (byte)nextByte);
                            readBuffer = temp;
                            totalBytesRead++;
                        }
                    }
                }

                byte[] buffer = readBuffer;
                if (readBuffer.Length != totalBytesRead)
                {
                    buffer = new byte[totalBytesRead];
                    Buffer.BlockCopy(readBuffer, 0, buffer, 0, totalBytesRead);
                }
                return buffer;
            }
            finally
            {
                if (stream.CanSeek)
                {
                    stream.Position = originalPosition;
                }
            }
        }



        public static void ClearNavigationStack()
        {
            var existingPages = Application.Current.MainPage.Navigation.NavigationStack.ToList();
            for (int i = 1; i < existingPages.Count; i++)
            {
                Application.Current.MainPage.Navigation.RemovePage(existingPages[i]);
            }
        }
    }
}
