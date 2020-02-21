using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;

using Core.Data.Models;

namespace Core.Data.Files
{


	[Serializable]
	public class UploadData
	{
		public DateTime creation_date;
		public HttpRequestMethod method = HttpRequestMethod.GET;
		public ModelType model_type;
		public string pk;
		public string json;

		private UploadData() { }
		public UploadData( HttpRequestMethod method, ModelType model_type, object pk = null, string json = null, DateTime creation_date = default(DateTime)) {
			this.method = method; this.model_type = model_type; this.pk = Convert.ToString(pk); this.json = json;
			if (creation_date.Equals(default(DateTime))) this.creation_date = DateTime.Now;
			else this.creation_date = creation_date;
		}
		public UploadData(HttpRequestMethod method, ModelType model_type, object pk = null, Dictionary<string, object> json = null, DateTime creation_date = default(DateTime)) 
			: this(method, model_type, pk, (json is null)?null:JsonSerializer.Serialize(json), creation_date) {}

	}
}
