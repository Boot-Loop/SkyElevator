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
		public IntergerField	id			{ get; set; } = new IntergerField(		name: "id",			is_required: true);
		public TextField		name		{ get; set; } = new TextField(			name: "name"		);
		public TextField		address		{ get; set; } = new TextField(			name: "address"		);
		public TextField		company		{ get; set; } = new TextField(			name: "company"		);
		public EmailField		email		{ get; set; } = new EmailField(			name: "email"		);
		public TextField		position	{ get; set; } = new TextField(			name: "position"	);
		public PhoneNumberField telephone	{ get; set; } = new PhoneNumberField(	name: "telephone"	);
		public NICField			nic			{ get; set; } = new NICField(			name: "nic"			);
		public WebSiteField		website		{ get; set; } = new WebSiteField(		name: "website"		);

		public ClientModel() { }
		public ClientModel(string name) { this.name.value = name; }

		/* overrides */
		override public string ToString()		=> this.name.value;
		override public ModelType getType()		=> ModelType.MODEL_CLIENT;
		override public bool matchPK(object pk) => id.value == Convert.ToInt64(pk);
		override public object getPK()			=> nic.value;
		override public void saveUpdates()		=> Application.getSingleton().clients_file.save();
		override public void saveNew() {
			Application.getSingleton().clients_file.data.clients.Add(this);
			Application.getSingleton().clients_file.save();
		}
	}
}
