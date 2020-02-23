using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
//using System.Security.Policy;

using Core.Data.Files;
using Core.Data.Models;
using Core.Utils;
using System.Security.Policy;
using System.Net.Http;

namespace Core.SebeClient
{

	public class SebeClient
	{
		private static SebeClient _singleton;
		public static  SebeClient singleton {
			get {
				if (_singleton is null) _singleton = new SebeClient();
				return _singleton;
			}
		}

		XmlFile<UploadData> cache_file { get; } = new XmlFile<UploadData>();
		SebeConnector connector { get; } = new SebeConnector("http://localhost:8000/");

		private SebeClient() { }

		public async Task login(string username, string password) => await connector.login(username, password);		
		public async Task logout() => await connector.logout();
		
		// TODO: after update delete file
		// NotLoggedInError, HttpNotFoundError, HttpBadRequestError, HttpRequestException // 1. not connected to internet
		public async Task upload() {
			while( anyUploadCacheLeft()) {
				loadUpdateCache();
				await sendRequest();
				deleteLastCache();
			}
		}


		/* private methods */
		private async Task sendRequest() {
			ModelType model_type = cache_file.data.model_type;
			string request_path = getRequestPath(model_type, cache_file.data.method);
			Dictionary<string, string> request_data = cache_file.data.getRequestData();
			Dictionary<string, string> attachments = cache_file.data.getAttachments();
			switch (cache_file.data.method)
			{
				case HttpRequestMethod.GET:
					throw new NotImplementedException();
				case HttpRequestMethod.POST:
					await connector.postRequest(request_path, request_data, attachments, drf:true);
					break;
				case HttpRequestMethod.PUT:
					await connector.putRequest(request_path, request_data, attachments, drf: true);
					break;
				case HttpRequestMethod.PATCH:
					await connector.patchRequest(request_path, request_data, attachments, drf:true);
					break;
				case HttpRequestMethod.DELETE:
					await connector.deleteRequest(request_path, drf: true);
					break;
			}
		}

		private void loadUpdateCache() {
			var file = Application.singleton.programe_data_file;
			if (file.data is null) throw new NullReferenceException("did you call Application.singleton.initialize()?");
			var cache_file_names = file.data.upload_cache_files;
			if (cache_file_names.Count > 0) {
				cache_file.path = Path.Combine( Core.Paths.UPLOAD_CACHE, cache_file_names[0] );
				cache_file.load();
			}
		}

		/// <summary>
		/// return if any cache files left
		/// </summary>
		/// <returns></returns>
		private void deleteLastCache() {
			var file = Application.singleton.programe_data_file;
			if (file.data is null) throw new NullReferenceException( "did you call Application.singleton.initialize()" );
			if (file.data.upload_cache_files.Count == 0) return;
			var cache_file_name = file.data.upload_cache_files[0];
			var path = Path.Combine(Core.Paths.UPLOAD_CACHE, cache_file_name);
			file.data.upload_cache_files.RemoveAt(0);
			file.save(); File.Delete(path);
		}

		private bool anyUploadCacheLeft() {
			return Application.singleton.programe_data_file.data.upload_cache_files.Count != 0;
		}

		private static string getRequestPath(ModelType model_type, HttpRequestMethod method, string pk = null)
		{
			switch (method)
			{
				case HttpRequestMethod.POST:
					return String.Join("/", Model.getAppName(model_type), Model.getVerbosePlural(model_type), "list") + "/";
				case HttpRequestMethod.GET:
					throw new InvalidOperationException("get method is not for update db");

				case HttpRequestMethod.PUT:
					goto case HttpRequestMethod.PATCH;
				case HttpRequestMethod.DELETE:
					goto case HttpRequestMethod.PATCH;
				case HttpRequestMethod.PATCH:
					if (pk is null) throw new ArgumentNullException("argument pk is required");
					return String.Join("/", Model.getAppName(model_type), Model.getVerboseSinglar(model_type), pk) + "/";
				default:
					throw new Exception("un handled request method");
			}
		}


	}
}

