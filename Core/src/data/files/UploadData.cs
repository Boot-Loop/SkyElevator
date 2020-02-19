using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Core.Data.Models;

namespace Core.Data.Files
{
	public enum RequestMethod
	{
		GET, POST, DELETE, PUT, PATCH
	}


	[Serializable]
	public class UploadData
	{
		public RequestMethod method = RequestMethod.GET;
		ModelType model_type;
		public string pk;
		public string json;

		private UploadData() { }
		public UploadData( RequestMethod method, ModelType model_type, string pk = null, string json = null) {
			this.method = method; this.model_type = model_type; this.pk = pk; this.json = json;
		}

	}
}
