using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml.Serialization;


namespace Core.Data.Files
{
	public class ProjectData
	{
		private ProjectData() { }
		public ProjectData( string project_name = null ) {
			this.project_name = project_name;
			foreach (FileTreeItem item in ProjectManager.getProjectTemplate()) {
				if (item is DirectoryItem) dirs.addDir((DirectoryItem)item);
				else if (item is FileItem) dirs.addFile((FileItem)item);
			}
			// dirs.getDir(ProjectManager.Dirs.INQUIRY_SHEET).getDir(ProjectManager.Dirs.CLIENT).addFile("inq_sheet_client1.txt");
		}

		public String project_name;
		public String application_version = Reference.VERSION;
		public string location;
		public string client_nic;
		public DateTime date;
		public DateTime creation_date;
		public DirectoryItem dirs = new DirectoryItem();
	}
}
