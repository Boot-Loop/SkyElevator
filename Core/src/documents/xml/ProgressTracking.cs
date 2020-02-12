using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Core.src.documents
{
	public class ClientProgress : IXmlData
	{
		public FloatField		total_amount_tobe_paid	= new FloatField();
		[XmlArrayItem("payment")]
		public List<FloatField> payments				= new List<FloatField>();
		public DateTimeField	arrived_date			= new DateTimeField();
		public DateTimeField	picked_up_date			= new DateTimeField();
		public FloatField		charge_for_late_picking	= new FloatField();
		public DateTimeField	unloaded_date			= new DateTimeField();

		public XmlDataType getType() => XmlDataType.PROGRESS_CLIENT;
	}

	public class SupplierProgress : IXmlData
	{

		public XmlDataType getType() => XmlDataType.PROGRESS_SUPPLIER;
	}

}
