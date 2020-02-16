using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Core.Data.Xml
{
	public class ClientProgress : IXmlData
	{
		public FloatField		total_amount_tobe_paid	{ get; set; } = new FloatField();
		[XmlArrayItem("payment")]
		public List<FloatField> payments				{ get; set; } = new List<FloatField>();
		public DateTimeField	arrived_date			{ get; set; } = new DateTimeField();
		public DateTimeField	picked_up_date			{ get; set; } = new DateTimeField();
		public FloatField		charge_for_late_picking	{ get; set; } = new FloatField();
		public DateTimeField	unloaded_date			{ get; set; } = new DateTimeField();

		public XmlDataType getType() => XmlDataType.PROGRESS_CLIENT;
	}

	public class SupplierProgress : IXmlData
	{

		public XmlDataType getType() => XmlDataType.PROGRESS_SUPPLIER;
	}

}
