using otp.Constants;
using otp.Models;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using otp.Model2;

namespace otp.Services
{
    public class OpenAIService : IOpenAIService
    {
        HttpClient client;

        public OpenAIService()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri(APIConstants.OpenAIUrl);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", APIConstants.OpenAIToken);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<string> AskQuestion(string query)
        {
            var completion = new CompletionRequest
            {
                Prompt = query
            };

            var body = JsonConvert.SerializeObject(completion);
            var content = new StringContent(body, Encoding.UTF8, "application/json");

            var response = await client.PostAsync(APIConstants.OpenAIEndpoint_Completions, content);

            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<CompletionResponse>(data);
                return result?.Choices?.FirstOrDefault()?.Text;
            }

            return default;
        }

        public async Task<string> CreateImage(string query)
        {
            var generation = new GenerationRequest
            {
                Prompt = query
            };

            var body = JsonConvert.SerializeObject(generation);
            var content = new StringContent(body, Encoding.UTF8, "application/json");

            var response = await client.PostAsync(APIConstants.OpenAIEndpoint_Generations, content);

            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<GenerationResponse>(data);
                return result?.Data?.FirstOrDefault()?.Url;
            }

            return default;
        }
    }
}
