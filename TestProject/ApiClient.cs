using Azure;

namespace EDMS.Common
{
	public class ApiClient
	{
		private readonly string _url;
		private readonly HttpClient _httpClient;

		public ApiClient(IConfiguration configuration, HttpClient httpClient)
		{
			_url = configuration["ApiSettings:ApiBaseUrl"];
			_httpClient = httpClient;
		}

		//static string _url = "https://recruitment.nblbd.com/api/HRMS/"; // need to get from appsettings.json file
		public async Task<string> LoginHrms(string payload)
		{
			var request = new HttpRequestMessage(HttpMethod.Post, _url + "getEmployeeInfo");
			var content = new StringContent(payload, null, "application/json");
			request.Content = content;
			var response = await _httpClient.SendAsync(request);
			response.EnsureSuccessStatusCode();
			using (var stream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false))
			{
				using (var reader = new StreamReader(stream))
				{
					return await reader.ReadToEndAsync().ConfigureAwait(false);
				}
			}
		}
	}
}
