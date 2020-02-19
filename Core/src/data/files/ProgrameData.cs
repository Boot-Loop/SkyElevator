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
	public class ProgrameData
	{
		public string default_proj_dir = Paths.DEFAULT_PROJ_DIR;
		[XmlArray("projects")]
		[XmlArrayItem("project")]
		public List<string> _recent_projects = new List<string>();

		public void addProjectPath(string path) {
			if (!File.Exists(Path.GetFullPath(path))) throw new FileNotFoundException("project file not found");
			if (!path.EndsWith(Reference.PROJECT_FILE_EXTENSION)) throw new InvalidPathError("project file path must ends with : " + Reference.PROJECT_FILE_EXTENSION);
			_recent_projects.Add(path);
		}

		public void setMostRecentProject(int index) {
			if (_recent_projects.Count <= index) throw new IndexOutOfRangeException();
			if (index != 0) {
				_recent_projects.Insert(0, _recent_projects[index]);
				_recent_projects.RemoveAt(index + 1);
			}
		}
	}
}
