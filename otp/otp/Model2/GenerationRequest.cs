using Newtonsoft.Json;


namespace otp.Models
{
	public class GenerationRequest
	{
		[JsonProperty("prompt")]
		public string Prompt { get; set; }

		[JsonProperty("n")]
		public int N { get; set; } = 1;

		[JsonProperty("size")]
		public string Size { get; set; } = "512x512";
	}
}