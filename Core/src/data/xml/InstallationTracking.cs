using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Data.Xml
{
	class MechanicalInstallation : IXmlData
	{
		public DateTimeField start_date		= new DateTimeField();
		public DateTimeField end_date		= new DateTimeField();

		public XmlDataType getType() => XmlDataType.INSTALLATION_MECHANICAL;
	}

	class ElectricalInstallation : IXmlData
	{
		public DateTimeField start_date				= new DateTimeField();
		public DateTimeField end_date				= new DateTimeField();
		public BoolField is_three_phase_available	= new BoolField(false);
		public DateTimeField checked_date			= new DateTimeField();

		public XmlDataType getType() => XmlDataType.INSTALLATION_ELECTRICAL;
	}
	
	class TestInstallation : IXmlData
	{
		public DateTimeField start_date		= new DateTimeField();
		public DateTimeField end_date		= new DateTimeField();

		public XmlDataType getType() => XmlDataType.INSTALLATION_TESTING;
	}

}
