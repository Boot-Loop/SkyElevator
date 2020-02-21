using Core.Data.Files;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Core.Data.Models
{
	[Serializable]
	public class ProjectModel : Model
	{
		public IntergerField	id				{ get; set; } = new IntergerField(	name: "id",				is_required: true, is_readonly: true );
		public TextField		name			{ get; set; } = new TextField(		name: "name"			);
		public TextField		location		{ get; set; } = new TextField(		name: "location"		);
		public IntergerField	client_id		{ get; set; } = new IntergerField(	name: "client_id",		is_required: true, is_readonly: true );
		public DateTimeField	date			{ get; set; } = new DateTimeField(	name: "date"			);
		public DateTimeField	creation_date	{ get; set; } = new DateTimeField(	name: "creation_date",	is_readonly: true );
		public IntergerField	project_type	{ get; set; } = new IntergerField(	name: "project_type",	is_readonly: true);

		public ProjectModel() {
			project_type.value = (int)ProjectManager.ProjectType.INSTALLATION;
		}
		public ProjectModel(string name) : this() { this.name.value = name;  }

		public ProjectManager.ProjectType getProjectType() => (ProjectManager.ProjectType)project_type.value;		
		public void setProjectType(ProjectManager.ProjectType type) => this.project_type.value = (long)type;
		

		/* overrides */
		public override string ToString()		=> this.name.value;
		public override ModelType getType()		=> ModelType.PROJECT_MODEL;
		public override bool matchPK(object pk) => id.value == Convert.ToInt64(pk);
		public override object getPK()			=>  id.value;
		public override void saveUpdates()		=> ProjectManager.singleton.project_file.save();

		public override void saveNew() {
			if (Application.getSingleton().is_proj_loaded) throw new InvalidOperationException("can't create project when another project already loaded");
			var project_file = ProjectManager.singleton.project_file;
			if (project_file.path is null) throw new Exception("path is null -> did you call Application.createNewProject()");
			project_file.data = new ProjectData(this.name.value, this);
			project_file.data.dirs.addFile(this.name.value + Reference.PROJECT_FILE_EXTENSION);
			project_file.save();
		}
	}
}
