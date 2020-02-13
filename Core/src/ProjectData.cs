using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml.Serialization;

using Core.src;

namespace Core.Data
{
	public class ProjectData
	{
		public ProjectData() {
			foreach ( DirectoryItem dir in ProjectManager.getProjectTemplate()) dirs.addDir(dir);
			// dirs.getDir(ProjectManager.Dirs.INQUIRY_SHEET).getDir(ProjectManager.Dirs.CLIENT).addFile("inq_sheet_client1.txt");
		}

		public String project_name;
		public String application_version = Application.VERSION;
		public DirectoryItem dirs = new DirectoryItem();
	}
}
