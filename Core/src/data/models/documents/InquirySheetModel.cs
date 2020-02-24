using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;


namespace Core.Data.Models
{
	[Serializable]
	public class InqurySheetTag
	{
		[XmlAttribute] public long	pk;
		[XmlAttribute] public string file_name;
		[XmlAttribute] public string doc;
		public string comments;
	}

	[Serializable]
	public class InquirySheetModel
	{

	}
}
