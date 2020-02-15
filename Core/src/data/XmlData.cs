using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Data
{
	public enum XmlDataType
	{
		PROGRESS_CLIENT,
		PROGRESS_SUPPLIER,
		INSTALLATION_MECHANICAL,
		INSTALLATION_ELECTRICAL,
		INSTALLATION_TESTING,
	}

	public interface IXmlData
	{
		XmlDataType getType();
	}

	/* xml file is at the utils directory */
}
