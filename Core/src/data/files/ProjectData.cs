using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml.Serialization;


namespace Core.Data.Files
{
	[Serializable]
	public class ProjectData
	{
		private ProjectData() { }
		public ProjectData( string project_name = null, ProjectManager.ProjectType project_type = ProjectManager.ProjectType.INSTALLATION ) {
			this.project_name = project_name;
			this.project_type = project_type;
			foreach (FileTreeItem item in ProjectManager.getProjectTemplate(project_type)) {
				if (item is DirectoryItem) dirs.addDir((DirectoryItem)item);
				else if (item is FileItem) dirs.addFile((FileItem)item);
			}
		}

		public String project_name;
		public ProjectManager.ProjectType project_type	= ProjectManager.ProjectType.INSTALLATION;
		public String application_version				= Reference.VERSION;
		public string location;
		public string client_nic;
		public DateTime date;
		public DateTime creation_date;
		public DirectoryItem dirs = new DirectoryItem();
	}
}
