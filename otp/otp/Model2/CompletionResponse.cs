using System.Collections.Generic;

namespace otp.Models
{
	public class CompletionResponse
	{
		public string Id { get; set; }
		public List<Choice> Choices { get; set; }
	}
}
