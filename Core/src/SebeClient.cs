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
using System.Text.RegularExpressions;

using Core.utils;
using Core.src;
/*
 every methods which connects to the backend throws this exception
		"System.Net.Http.HttpRequestException"
 */

/* TODOs:
 * 
 * 
 * document the code with possible throws 
 * 
 * save resp with mime type, image -> binary write
 * cookie expiry time
 * response timeout
 * 
 * impl:method -> cler cookies, caches, ...
 * log and save resp, other with time ( resp names -> resp_time.mime_type )
 * 
 * delete cookie after logout
 * 
 * BUGS:
 * 
 * calling any method( drf: true ) when logged out/ no more session id would throws an error!
 * if there's no PROGRAMME_DATA_PATH directory -> it makes an error
 * 
 */

namespace SebeClient
{
	public class Client
    {
		private class Urls
		{
			public static string HOST		= "http://localhost:8000/";
			public static string LOGIN		= "api-login/";
			public static string LOGOUT		= "api-logout/";
		}

		// static
		private static Logger logger = new Logger();

		private Uri uri;
		private CookieContainer cookies;
		private HttpClient http_client;
		private HttpResponseMessage response;

		// TODO:
		private string resp_log_path  = Path.Combine(Paths.SEBE_CLIENT, "resp.html");
		private string cookie_path    = Path.Combine(Paths.SEBE_CLIENT, "cookies.dat");

		public Client(string uri)
		{
			try {
				cookies = loadCookies();
			}
			catch (FileNotFoundException) {
				cookies = new CookieContainer();
			}

			this.uri = new Uri(uri);
			HttpClientHandler handler = new HttpClientHandler();
			handler.CookieContainer = cookies;
			this.http_client = new HttpClient(handler);
			logger = new Logger();
		}
		public Client() : this(Urls.HOST) { }

		public void setTimeout(double seconds) => http_client.Timeout = TimeSpan.FromSeconds(seconds);
		public void dispose() => http_client.Dispose();

		public async Task getRequest(string path) {
			response = await http_client.GetAsync( new Uri(uri, path ) );
			await saveResponse();
			checkResponse(path);
		}
		/* private overloaded requests */
		private async Task postRequest(string path, HttpContent content = null, bool drf = false)
		{
			if (drf) await getDrfCsrfToken(path);
			response = await http_client.PostAsync(new Uri(uri, path), content);
			await saveResponse();
			checkResponse(path);
		}
		private async Task putRequest(string path, HttpContent content = null, bool drf = false)
		{
			if (drf) await getDrfCsrfToken(path);
			response = await http_client.PutAsync(new Uri(uri, path), content);
			await saveResponse();
			checkResponse(path);
		}
		private async Task patchRequest(string path, HttpContent content=null, bool drf = false)
		{
			if (drf) await getDrfCsrfToken(path);
			var method = new HttpMethod("PATCH");
			var request = new HttpRequestMessage(method, new Uri(uri, path));
			request.Content = content;
			response = await http_client.SendAsync(request);
			await saveResponse();
			checkResponse(path);
		}

		/* low level requests */
		public async Task deleteRequest(string path, bool drf = false) {
			if (drf) await getDrfCsrfToken(path);
			this.response = await this.http_client.DeleteAsync(new Uri(this.uri, path));
			await saveResponse();
			checkResponse(path);
		}

		public async Task postRequest(string path, Dictionary<string,string> post_data = null, Dictionary<string, string> attach_files = null, bool drf = false) {		
			await postRequest(path, makeHttpContent(post_data, attach_files), drf);
		}

		public async Task putRequest(string path, Dictionary<string, string> put_data = null, Dictionary<string, string> attach_files = null, bool drf = false) {
			await putRequest(path, makeHttpContent(put_data, attach_files), drf);
		}

		public async Task patchRequest(string path, Dictionary<string, string> put_data = null, Dictionary<string, string> attach_files = null, bool drf = false) {
			await patchRequest(path, makeHttpContent(put_data, attach_files), drf);
		}

		/* high level requests */
		public async Task login(string username, string password) {
			await this.getRequest(Urls.LOGIN);
			checkResponse(Urls.LOGIN);
			string csrfmiddlewaretoken = this.getCookieValue("csrftoken"); // csrfmiddlewaretoken, sessionid
			if (csrfmiddlewaretoken is null) throw new Exception("csrftoken not found in cookies");
			Dictionary<string, string> post_data = new Dictionary<string, string>();
			post_data.Add("csrfmiddlewaretoken", csrfmiddlewaretoken);
			post_data.Add("username", username);
			post_data.Add("password", password);
			await this.postRequest(Urls.LOGIN, post_data);
			checkResponse(Urls.LOGIN);
			logger.logSuccess("successfully logged in");
		}

		public async Task logout() {
			await getRequest(Urls.LOGOUT);
			checkResponse(Urls.LOGOUT);
		}

		/********* getters and setters **********/

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
		public string getCookieValue(string key){
			foreach (Cookie cookie in getCookies())
				if (cookie.Name == key) return cookie.Value;
			return null;
		}

		// if response is null error!
		public int getStatusCode() => (int)response.StatusCode;
		public string getResponseMimeType() => response.Content.Headers.ContentType.ToString();
		public bool hasResponseContentDisposition() => !(response.Content.Headers.ContentDisposition is null);
		public string getResponseAttachmentName() => response.Content.Headers.ContentDisposition.FileName;
		// TODO: contentDisposition may be null -> if no file attached handle

		public async Task<string> getResponseString() => await response.Content.ReadAsStringAsync();
		public async Task<Stream> getResponseStream() => await response.Content.ReadAsStreamAsync();
		public async Task<byte[]> getResponseBytes() => await response.Content.ReadAsByteArrayAsync();

		/// <summary>
		/// save the response to the path(<c>file_path</c>). path is the full path to the file with extension (jpg, jpeg, gif, ...).
		/// <para/>
		/// for the extension use <see cref="getResponseMimeType"/>
		/// </summary>
		/// <param name="file_path"></param>
		/// <param name="mode"></param>
		/// <returns></returns>
		public async Task saveResponseBytes(string file_path, FileMode mode = FileMode.OpenOrCreate){
			using (Stream streamToReadFrom = await response.Content.ReadAsStreamAsync())
				using (Stream streamToWriteTo = File.Open(file_path, mode))
					await streamToReadFrom.CopyToAsync(streamToWriteTo);
		}

		/// <summary>
		/// save the file attached to the response to the path(<c>dir_path</c>). file name and extension will get from response header.
		/// <para/>
		/// use method <see cref="hasResponseContentDisposition"/> to check if any files were attached to the response. 
		/// <para/> 
		/// if the response is a binary file use <see cref="saveResponseBytes(string, FileMode)"/>
		/// </summary>
		/// <param name="dir_path">path to the directory (file name and extension will added from the response)</param>
		/// <param name="mode">the file mode to open</param>
		/// <returns></returns>
		public async Task saveResponseAttachment( string dir_path, FileMode mode = FileMode.OpenOrCreate ) 
		{
			string fileToWriteTo = System.IO.Path.Combine(dir_path, getResponseAttachmentName());
			if (File.Exists(fileToWriteTo)) logger.logWarning("file already exists overriding : "+ fileToWriteTo);
			using (Stream streamToReadFrom = await response.Content.ReadAsStreamAsync())
				using (Stream streamToWriteTo = File.Open(fileToWriteTo, mode))
					await streamToReadFrom.CopyToAsync(streamToWriteTo);
		}

		// TODO:
		public CookieContainer loadCookies() {
			var formatter = new SoapFormatter();
			CookieContainer retrievedCookies = null;
			using (Stream stream = File.OpenRead(cookie_path))
				retrievedCookies = (CookieContainer)formatter.Deserialize(stream);
			return retrievedCookies;
		}
		///////////////////////////// PRIVARE METHODS ///////////////////////////////////
		
		// used in getDrfCsrfToken
		public static string getFirstMatch(string text, string expr) {
			MatchCollection mc = Regex.Matches(text, expr);
			foreach (Match m in mc) {
				return m.ToString();
			}
			return null;
		}

		private async Task<KeyValuePair<string, string>> getDrfCsrfToken(string path)
		{
			this.response = await http_client.GetAsync(new Uri(this.uri, path + "?format=api"));
			await saveResponse();
			checkResponse(path + "?format=api");

			string token_re = @"csrfHeaderName\s*:\s*"".*""\s*,\s*csrfToken\s*:\s*"".*""";
			string match = getFirstMatch(await getResponseString(), token_re);
			if (match is null) { throw new Exception("no matching re found for : "+ token_re); }
			string[] keys_and_values = match.Split( new[] { ',', ':'});
			if (keys_and_values.Length != 4) {
				throw new Exception("invalie size of csrf header json string");
			}
			string csrf_header = keys_and_values[1].Trim();
			csrf_header = csrf_header.Substring(1, csrf_header.Length-2 );
			string csrf_token = keys_and_values[3].Trim();
			csrf_token = csrf_token.Substring(1, csrf_token.Length- 2);
			logger.logSuccess( "drf csrf token found : "+csrf_token);
			http_client.DefaultRequestHeaders.Add(csrf_header, csrf_token);
			return new KeyValuePair<string, string>(csrf_header, csrf_token);
		}

		private HttpContent makeHttpContent(Dictionary<string, string> kv_data, Dictionary<string, string> attach_files)
		{
			var form_data = new MultipartFormDataContent();

			if (kv_data != null) {

				foreach (var kv in kv_data) {
					StringContent str_content = new StringContent(kv.Value);
					form_data.Add(str_content, kv.Key);
				}
			}

			if (!(attach_files is null)) {
				foreach (var kv in attach_files) {
					if (!File.Exists(kv.Value)) logger.logError("attach file not exists : " + kv.Value);
					ByteArrayContent fileContent = new ByteArrayContent(File.ReadAllBytes(kv.Value));
					form_data.Add(fileContent, kv.Key, Path.GetFileName(kv.Value));
				}
			}

			return form_data;
		}

		private void checkResponse(string path) {
			switch (getStatusCode())
			{
				case 404:
					{
						logger.logError("Error: 404 not found : " + path);
						throw new HttpNotFoundError("path : " + path);
					}
				case 307:
					{
						logger.logError("Error: not logged in (cookie may expired)");
						throw new NotLoggedInError();
					}
				case 400:
					{
						logger.logError("Error: bad request : " + path);
						throw new HttpBadRequestError("path :" + path);
					}
			}
		}


		private async Task saveResponse() // if error no app_data_folder, debug save 
		{
			// TODO: save with mime type and binary for pdf, image, ...
			string response_string = await getResponseString();
			File.WriteAllText(resp_log_path, response_string);
			saveCookies();
		}

		private void saveCookies() {
			var formatter = new SoapFormatter();
			using (Stream s = File.Create(cookie_path))
				formatter.Serialize(s, cookies);
		}
	}
}
