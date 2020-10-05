using System;
using RestSharp;

namespace RestRunner
{
	class Program
	{
		static string serviceURL = "https://localhost:5001";

		static void Main(string[] args)
		{
			var testUser = "User1";
			var testPassword = "Password1";

			var generatedToken = Login(testUser, testPassword);

			if (generatedToken.Length == 124)  // A valid token has a length of 124.
			{
				var result = Validate(generatedToken, testUser);

				Console.WriteLine(result);
			}
			else
			{
				Console.WriteLine($"[Login Error] {generatedToken}");
			}
		}

		static string Login(string userName, string password)
		{
			var client = new RestClient($"{serviceURL}/Authentication/login");
			client.RemoteCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;

			var request = new RestRequest(Method.POST);
			request.AddHeader("user-agent", "vscode-restclient");
			request.AddHeader("content-type", "application/json");

			string restBody = $"{{\"username\": \"{userName}\",\"password\": \"{password}\"}}";
			request.AddParameter("application/json", restBody, ParameterType.RequestBody);
			IRestResponse response = client.Execute(request);

			return response.Content.Replace("\"", "");
		}

		static string Validate(string token, string userName)
		{
			var client = new RestClient($"{serviceURL}/Authentication/verify?username={userName}&token={token}");
			client.RemoteCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;
			var request = new RestRequest(Method.GET);
			request.AddHeader("user-agent", "vscode-restclient");
			request.AddHeader("content-type", "application/json");
			IRestResponse response = client.Execute(request);

			return $"[{response.StatusDescription}] {response.Content}".Trim();
		}
	}
}
