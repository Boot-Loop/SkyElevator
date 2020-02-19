using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Data.Models
{
	[Serializable]
	public class ProjectModel : Model
	{
		public TextField					name			{ get; set; } = new TextField(					name: "name");
		public TextField					location		{ get; set; } = new TextField(					name: "location");
		public DropDownField<ClientModel>	client			{ get; set; } = new DropDownField<ClientModel>( name: "client");
		public DateTimeField				date			{ get; set; } = new DateTimeField(				name: "date");
		public DateTimeField				creation_date	{ get; set; } = new DateTimeField(				name: "creation_date");
		public ProjectManager.ProjectType	project_type	{ get; set; } = ProjectManager.ProjectType.INSTALLATION;

		public ProjectModel() {
			client.setItems( Application.getSingleton().getClientsDropDownList() );
		}
		public ProjectModel(string name) : this() { this.name.value = name;  }
		public override string ToString() => this.name.value;

	}
}
