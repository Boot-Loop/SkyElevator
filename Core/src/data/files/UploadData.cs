using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;

using Core.Data.Models;
using System.Xml.Serialization;

namespace Core.Data.Files
{
	// TODO: use this every where
	[Serializable]
	public class KVPair
	{
		[XmlAttribute] public string key { get; set; }
		[XmlAttribute] public string value { get; set; }
	}

	[Serializable]
	public class UploadData
	{
		public DateTime creation_date;
		public HttpRequestMethod method = HttpRequestMethod.GET;
		public ModelType model_type;
		public string pk;
		[XmlArray] [XmlArrayItem("pair")]
		public List<KVPair> request_data = new List<KVPair>();
		[XmlArray] [XmlArrayItem("pair")]
		public List<KVPair> attachments = new List<KVPair>();
		public string response_data;

		private UploadData() { }
		public UploadData( HttpRequestMethod method, ModelType model_type, object pk = null, Dictionary<string, string> request_data = null, Dictionary<string, string> attachments = null, DateTime creation_date = default(DateTime)) {
			this.method = method; this.model_type = model_type; this.pk = Convert.ToString(pk); 
			if (creation_date.Equals(default(DateTime))) this.creation_date = DateTime.Now;
			else this.creation_date = creation_date;

			if (request_data!= null) foreach( var p in request_data)  this.request_data.Add(	new KVPair() { key = p.Key, value = p.Value }	);
			if (attachments != null) foreach( var p in attachments )  this.attachments.Add(	new KVPair() { key = p.Key, value = p.Value }	);
			
		}

		public Dictionary<string, string> getRequestData() {
			if (request_data.Count == 0) return null;
			var ret = new Dictionary<string, string>();
			foreach( var pair in request_data) ret.Add(pair.key, pair.value);
			return ret;
		}

		public Dictionary<string, string> getAttachments() {
			if (attachments.Count == 0) return null;
			var ret = new Dictionary<string, string>();
			foreach (var pair in attachments) ret.Add( pair.key, pair.value );
			return ret;
		}

	}
}
