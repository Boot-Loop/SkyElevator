using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Data.Models
{
	[Serializable]
	public class ClientModel : Model
	{
		public TextField		name		{ get; set; } = new TextField(		 name:"name"		);
		public TextField		address		{ get; set; } = new TextField(		 name:"address"		);
		public TextField		company		{ get; set; } = new TextField(		 name:"company"		);
		public EmailField		email		{ get; set; } = new EmailField(		 name:"email"		);
		public TextField		position	{ get; set; } = new TextField(		 name:"position"	);
		public PhoneNumberField telephone	{ get; set; } = new PhoneNumberField( name:"telephone"	);
		public NICField			nic			{ get; set; } = new NICField(		 name:"nic"			);
		public WebSiteField		website		{ get; set; } = new WebSiteField(	 name:"website"		);

		public ClientModel() { }
		public ClientModel(string name) { this.name.value = name; }

		override public string ToString() { return this.name.value; }
	}
}
