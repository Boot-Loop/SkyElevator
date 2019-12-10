using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace SebeClient
{
	public class Request
	{
		public static async Task CookeiTest()
		{
			CookieContainer cookies = new CookieContainer();
			HttpClientHandler handler = new HttpClientHandler();
			handler.CookieContainer = cookies;

			HttpClient client = new HttpClient(handler);
			Uri uri = new Uri("http://localhost:8000/accounts/login");

			HttpResponseMessage response = await client.GetAsync(uri);

			string responseContent = await response.Content.ReadAsStringAsync();
			Console.WriteLine(responseContent);

			IEnumerable<Cookie> responseCookies = cookies.GetCookies(uri).Cast<Cookie>();
			foreach (Cookie cookie in responseCookies)
			{
				Console.WriteLine(cookie.Name + ": " + cookie.Value);
				Console.WriteLine($"domain: {cookie.Domain} | path: {cookie.Path} | expr: {cookie.Expires}");
			}

			client.Dispose();
		}

	}
}
