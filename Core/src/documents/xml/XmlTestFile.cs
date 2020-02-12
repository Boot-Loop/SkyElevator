using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Core.src.documents
{
	public class XmlTestFile
	{
		public TextField name	= new TextField("Dexter");
		public FloatField price	= new FloatField(123.456);
		public DateTimeField date = new DateTimeField( new DateTime(2020, 01, 12) );
		public BoolField is_3_pahse = new BoolField(false);

		[XmlArrayItem("list_item")]
		public List<IntergerField> list = new List<IntergerField>() {
			new IntergerField(1),
			new IntergerField(2),
			new IntergerField(3),
		};
	}
}
