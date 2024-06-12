using System.Threading.Tasks;

namespace otp.Services
{
	public interface IOpenAIService
	{
		Task<string> AskQuestion(string query);

		Task<string> CreateImage(string query);
	}
}
