using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Data.Models
{
	public class ClientModel
	{
		public TextField		name				= new TextField();
		public TextField		address				= new TextField();
		public TextField		company				= new TextField();
		public EmailField		email				= new EmailField();
		public TextField		position			= new TextField();
		public PhoneNumberField telephone_number	= new PhoneNumberField();
		public NICField			nic					= new NICField();
		public WebSiteField		website				= new WebSiteField();

		public ClientModel() { }
		public ClientModel(string name) { this.name.value = name; }

		override public string ToString() { return this.name.value; }
	}
}
