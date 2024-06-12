using Newtonsoft.Json;


namespace otp.Model2
{
    public class CompletionRequest
    {
        [JsonProperty("model")]
        public string Model { get; set; } = "gpt-3.5-turbo-instruct";

        [JsonProperty("prompt")]
        public string Prompt { get; set; }

        [JsonProperty("temperature")]
        public double Temperature { get; set; } = 0;

        [JsonProperty("max_tokens")]
        public int MaxTokens { get; set; } = 100;
    }
}
