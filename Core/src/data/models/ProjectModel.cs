using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Data.Models
{
	public class ProjectModel
	{
		public TextField		name				= new TextField();
		public TextField		location			= new TextField();
		public DropDownField<ClientModel> client	= new DropDownField<ClientModel>(
															new ObservableCollection<ClientModel>() { 
																new ClientModel("client1"), 
																new ClientModel("client1"), 
															}
														); // TODO: build and set client model list here
		public DateTimeField	date				= new DateTimeField();
		public DateTimeField	creation_date		= new DateTimeField();

		public ProjectModel() { }
		public ProjectModel(string name) { this.name.value = name;  }

		public override string ToString() => this.name.value;
	}
}
