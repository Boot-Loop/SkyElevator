using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml.Serialization;
using Core.Data.Models;

namespace Core.Data.Files
{
	[Serializable]
	public class ProjectData
	{

		public String application_version	{ get; set; } = Reference.VERSION;
		public ProjectModel project_model	{ get; set; } // = new ProjectModel();
		// public String project_name;
		// public ProjectManager.ProjectType project_type	= ProjectManager.ProjectType.INSTALLATION;
		// public string location;
		// public long client_id;
		// public DateTime date;
		// public DateTime creation_date;

		public DirectoryItem dirs = new DirectoryItem();

		private ProjectData() { }
		public ProjectData( string project_name , ProjectModel project_model) { //  ProjectManager.ProjectType project_type = ProjectManager.ProjectType.INSTALLATION 
			project_model.name.value = project_name;
			foreach (FileTreeItem item in ProjectManager.getProjectTemplate(project_model.getProjectType())) {
				if (item is DirectoryItem) dirs.addDir((DirectoryItem)item);
				else if (item is FileItem) dirs.addFile((FileItem)item);
			}
		}
	}
}
