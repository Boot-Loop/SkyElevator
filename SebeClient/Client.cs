using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization.Formatters.Soap;

namespace SebeClient
{
    public class Client
    {
		public static async Task connect()
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

		static void serialize(CookieContainer cookies)
		{
			var formatter = new SoapFormatter();
			string file = Path.Combine(Directory.GetCurrentDirectory(), "cookies.dat");

			using (Stream s = File.Create(file))
				formatter.Serialize(s, cookies);
		}

		static CookieContainer deSerialize()
		{
			var formatter = new SoapFormatter();
			string file = Path.Combine(Directory.GetCurrentDirectory(), "cookies.dat");

			CookieContainer retrievedCookies = null;
			using (Stream s = File.OpenRead(file))
				retrievedCookies = (CookieContainer)formatter.Deserialize(s);

			return retrievedCookies;
		}
	}
}
