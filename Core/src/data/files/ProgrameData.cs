using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml.Serialization;

using Core.Utils;
using System.Collections.ObjectModel;

namespace Core.Data.Files
{
	[Serializable]
	public class ProgrameData
	{
		[Serializable]
		public class ProjectViewData
		{
			[XmlAttribute] public string name;
			[XmlAttribute] public string client_name;
			[XmlAttribute] public string path;

			public ProjectViewData() { }
			public ProjectViewData( string name, string client_name, string path) {
				this.name = name; this.client_name = client_name; this.path = path;
			}
		}

		public string default_proj_dir = Paths.DEFAULT_PROJ_DIR;
		[XmlArray("projects")]
		[XmlArrayItem("project")]
		public ObservableCollection<ProjectViewData> recent_projects = new ObservableCollection<ProjectViewData>();

		[XmlArray("uploads_caches")]
		[XmlArrayItem("cache")]
		public List<string> upload_cache_files = new List<string>();

		public void addProject(string name, string client_name, string path) {
			if (!File.Exists(Path.GetFullPath(path))) { Logger.logThrow(new FileNotFoundException("project file not found")); }
			if (!path.EndsWith(Reference.PROJECT_FILE_EXTENSION)) { Logger.logThrow(new InvalidPathError("project file path must ends with : " + Reference.PROJECT_FILE_EXTENSION)); }
			//if (!recent_projects.Contains(path)) recent_projects.Add(path); // TODO:
			ProjectViewData data = new ProjectViewData(name, client_name, path);
			recent_projects.Insert(0, data);
		}

		public void cleanProjectPaths() {
			List<ProjectViewData> to_remove = new List<ProjectViewData>();
			foreach( var data in recent_projects) {
				if (!File.Exists(Path.GetFullPath(data.path))) {
					to_remove.Add(data);
				}
			}
			foreach( var data in to_remove) {
				Logger.logger.logWarning("project file path was not found -> removing the path : "+ data.path );
				recent_projects.Remove(data);
			}
		}

		public void setMostRecentProject(int index) {
			if (recent_projects.Count <= index) throw new IndexOutOfRangeException();
			if (index != 0) {
				recent_projects.Insert(0, recent_projects[index]);
				recent_projects.RemoveAt(index + 1);
			}
		}
	}
}
