using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.src.documents
{
	public class ClientModel : IXmlData
	{
		public TextField		name				= new TextField();
		public TextField		address				= new TextField();
		public TextField		company				= new TextField();
		public EmailField		email				= new EmailField();
		public TextField		position			= new TextField();
		public PhoneNumberField telephone_number	= new PhoneNumberField();
		public NICField			nic					= new NICField();
		public WebSiteField		website				= new WebSiteField();

		public XmlDataType getType() => XmlDataType.MODEL_CLIENT;
	}

	// TODO: client list as xml file
	// public class ClientList
	// {
	// 	private List<ClientModel> client_model_list = new List<ClientModel>();
	// }
}
