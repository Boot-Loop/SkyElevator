using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization.Formatters.Soap;

/* TODOs:
 * save resp with mime type, image -> binary write
 * cookie expiry time
 * response timeout
 * 
 * impl:method -> cler cookies, caches, ...
 * log and save resp, other with time ( resp names -> resp_time.mime_type )
 * impl:put delete http_methods
 * 
 * test pdf, image, docx, other types of files
 * 
 */

namespace SebeClient
{
	public class Client
    {
		// static
		public static readonly string APP_DATA_PATH = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
		public static readonly string PROGRAMME_DATA_PATH = Path.Combine(APP_DATA_PATH, "SkyElevator/SebeClient/");

		private Uri uri;
		private CookieContainer cookies;
		private HttpClient http_client;
		private HttpResponseMessage response;

		private string resp_log_path  = Path.Combine(PROGRAMME_DATA_PATH, "resp.html");
		private string error_log_path = Path.Combine(PROGRAMME_DATA_PATH, "errors.log");
		private string cookie_path    = Path.Combine(PROGRAMME_DATA_PATH, "cookies.dat");


		public static void Init()
		{
			Directory.CreateDirectory(PROGRAMME_DATA_PATH);
		}

		public Client( string uri = "http://localhost:8000/")
		{
			Init(); // TODO: move this

			try {
				this.cookies = loadCookies();
			}
			catch (FileNotFoundException) {
				this.cookies = new CookieContainer();
			}

			this.uri = new Uri(uri);			
			HttpClientHandler handler = new HttpClientHandler();
			handler.CookieContainer = cookies;
			this.http_client = new HttpClient(handler);
		}

		public async Task get(string path) // path = /accounts/login/
		{
			this.response = await this.http_client.GetAsync( new Uri(this.uri, path ) );
			await saveResponse();
		}

		public async Task post(string path, List<KeyValuePair<string,string>> post_data = null)
		{
			if (post_data is null)
			{
				post_data = new List<KeyValuePair<string, string>>();
			}
			FormUrlEncodedContent content = new FormUrlEncodedContent(post_data);
			this.response = await this.http_client.PostAsync(new Uri(this.uri, path), content);
			await saveResponse();
		}

		public async Task login(string path, string username, string password)
		{
			await this.get(path);
			string csrfmiddlewaretoken = this.getCookieValue("csrftoken"); // csrfmiddlewaretoken, sessionid
			List<KeyValuePair<string, string>> post_data = new List<KeyValuePair<string, string>>();
			post_data.Add(new KeyValuePair<string, string>("csrfmiddlewaretoken", csrfmiddlewaretoken));
			post_data.Add(new KeyValuePair<string, string>("username", username));
			post_data.Add(new KeyValuePair<string, string>("password", password));
			await this.post(path, post_data);
		}


		public IEnumerable<Cookie> getCookies()
		{
			IEnumerable<Cookie> responseCookies = this.cookies.GetCookies(this.uri).Cast<Cookie>();
			/*
			foreach (Cookie cookie in responseCookies)
			{
				Console.WriteLine(cookie.Name + ": " + cookie.Value);
				Console.WriteLine($"domain: {cookie.Domain} | path: {cookie.Path} | expr: {cookie.Expires}");
			}
			*/
			return responseCookies;
		}
		public string getCookieValue(string key)
		{
			foreach (Cookie cookie in this.getCookies()){
				if (cookie.Name == key) return cookie.Value;
			}
			return null;
		}

		public string getResponseMimeType() // application/json, text/html, ...
		{
			return response.Content.Headers.ContentType.ToString();
		}

		public async Task<string> getResponseString()
		{
			return await response.Content.ReadAsStringAsync();
		}

		public void dispose()
		{
			this.http_client.Dispose();
		}

		public void saveCookies()
		{
			var formatter = new SoapFormatter();
			using (Stream s = File.Create(this.cookie_path))
				formatter.Serialize(s, cookies);
		}

		public CookieContainer loadCookies() // if error no app_data_folder
		{
			var formatter = new SoapFormatter();
			CookieContainer retrievedCookies = null;
			using (Stream s = File.OpenRead(this.cookie_path))
				retrievedCookies = (CookieContainer)formatter.Deserialize(s);

			return retrievedCookies;
		}
		///////////////////////////// PRIVARE METHODS ///////////////////////////////////

		private async Task saveResponse() // if error no app_data_folder
		{
			// TODO: save with mime type and binary for pdf, image, ...
			string response_string = await this.getResponseString();
			File.WriteAllText(this.resp_log_path, response_string);
			saveCookies();
		}
	}
}
