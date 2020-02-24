using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Core.Data.Models
{
	[Serializable]
	public class SalesAgreementTag
	{
		[XmlAttribute] public long pk;
		[XmlAttribute] public string file_name;
		[XmlAttribute] public string doc;
		public string comments;
	}

	public class SalesAgreementModel
	{

	}
}
