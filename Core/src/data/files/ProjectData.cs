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

		public string application_version = Reference.VERSION;
		public ProjectModel project_model;

		public DirectoryItem dirs = new DirectoryItem();

		private ProjectData() { }
		public ProjectData( string project_name , ProjectModel project_model) { //  ProjectManager.ProjectType project_type = ProjectManager.ProjectType.INSTALLATION 
			this.project_model = project_model;
			foreach (FileTreeItem item in ProjectManager.getProjectTemplate(project_model.getProjectType())) {
				if (item is DirectoryItem) dirs.addDir((DirectoryItem)item);
				else if (item is FileItem) dirs.addFile((FileItem)item);
			}
		}
	}
}
