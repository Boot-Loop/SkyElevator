using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization.Formatters.Soap;
using System.Net.Http.Headers;

using SebeClient;

/*
 every methods which connects to the bachend throws this exception
		"System.Net.Http.HttpRequestException"
 */

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
 * delete cookie after logout
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

		public async Task get(string path) {
			this.response = await this.http_client.GetAsync( new Uri(this.uri, path ) );

			await saveResponse();
		}

		public HttpClient getHttpClient(){
			return this.http_client;
		}
		
		public async Task post(string path, List<KeyValuePair<string,string>> post_data = null, bool drf = false) // drf : django rest framework
		{
			if (drf) await getDrfCsrfToken(path); 

			if (post_data is null) {
				post_data = new List<KeyValuePair<string, string>>();
			}
			FormUrlEncodedContent content = new FormUrlEncodedContent(post_data);
			this.response = await this.http_client.PostAsync(new Uri(this.uri, path), content);
			await saveResponse();
		}

		public async Task put(string path, List<KeyValuePair<string, string>> post_data = null, bool drf = false)
		{
			if (drf) await getDrfCsrfToken(path);

			if (post_data is null) {
				post_data = new List<KeyValuePair<string, string>>();
			}
			FormUrlEncodedContent content = new FormUrlEncodedContent(post_data);
			this.response = await this.http_client.PutAsync(new Uri(this.uri, path), content);
			await saveResponse();
		}

		public async Task delete(string path, bool drf = false)
		{
			if (drf) await getDrfCsrfToken(path);

			this.response = await this.http_client.DeleteAsync(new Uri(this.uri, path));
			await saveResponse();
		}

		
		public async Task login(string path, string username, string password)
		{
			await this.get(path);
			string csrfmiddlewaretoken = this.getCookieValue("csrftoken"); // csrfmiddlewaretoken, sessionid
			if (csrfmiddlewaretoken is null) throw new Exception("csrftoken not found in cookies");
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

		// if no response available returns -1
		public int getStatusCode() { 
			if (this.response is null) return -1;
			return (int)this.response.StatusCode;
		}

		public string getResponseMimeType() // application/json, text/html, ...
		{
			return response.Content.Headers.ContentType.ToString();
		}

		public async Task<string> getResponseString()
		{
			return await response.Content.ReadAsStringAsync();
		}

		public void dispose(){
			this.http_client.Dispose();
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
		
		private async Task<KeyValuePair<string, string>> getDrfCsrfToken(string path)
		{
			try { 
				this.response = await http_client.GetAsync(new Uri(this.uri, path + "?format=api"));
			} catch (Exception err) { throw err; }

			if (getStatusCode() != 200){
				throw new Exception("getDrfCsrfToken get api request was not success");
			}
			string token_re = @"csrfHeaderName\s*:\s*"".*""\s*,\s*csrfToken\s*:\s*"".*""";
			string match = Utils.getFirstMatch(await getResponseString(), token_re);
			if (match is null) { throw new Exception("no matching re found for : "+ token_re); }
			string[] keys_and_values = match.Split( new[] { ',', ':'});
			if (keys_and_values.Length != 4) {
				throw new Exception("invalie size of csrf header json string");
			}
			string csrf_header = keys_and_values[1].Trim();
			csrf_header = csrf_header.Substring(1, csrf_header.Length-2 );
			string csrf_token = keys_and_values[3].Trim();
			csrf_token = csrf_token.Substring(1, csrf_token.Length- 2);
			http_client.DefaultRequestHeaders.Add(csrf_header, csrf_token);
			return new KeyValuePair<string, string>(csrf_header, csrf_token);
		}


		private async Task saveResponse() // if error no app_data_folder, debug save 
		{
			// TODO: save with mime type and binary for pdf, image, ...
			string response_string = await this.getResponseString();
			File.WriteAllText(this.resp_log_path, response_string);
			saveCookies();
		}

		private void saveCookies(){
			var formatter = new SoapFormatter();
			using (Stream s = File.Create(this.cookie_path))
				formatter.Serialize(s, cookies);
		}
	}
}
