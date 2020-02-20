﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml.Serialization;

using Core.Utils;

namespace Core.Data.Files
{
	[Serializable]
	public class ProgrameData
	{
		public string default_proj_dir = Paths.DEFAULT_PROJ_DIR;
		[XmlArray("projects")]
		[XmlArrayItem("project")]
		public List<string> _recent_projects = new List<string>();

		[XmlArray("uploads_caches")]
		[XmlArrayItem("cache")]
		public List<string> upload_cache_files = new List<string>();

		public void addProjectPath(string path) {
			if (!File.Exists(Path.GetFullPath(path))) { Logger.logThrow(new FileNotFoundException("project file not found")); }
			if (!path.EndsWith(Reference.PROJECT_FILE_EXTENSION)) { Logger.logThrow(new InvalidPathError("project file path must ends with : " + Reference.PROJECT_FILE_EXTENSION)); }
			if (!_recent_projects.Contains(path)) _recent_projects.Add(path);
		}

		public void cleanProjectPaths() {
			List<string> to_remove = new List<string>();
			foreach( string path in _recent_projects) {
				if (!File.Exists(Path.GetFullPath(path))) {
					to_remove.Add(path);
				}
			}
			foreach( string path in to_remove) {
				Logger.logger.logWarning("project file path was not found -> removing the path : "+ path );
				_recent_projects.Remove(path);
			}
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
