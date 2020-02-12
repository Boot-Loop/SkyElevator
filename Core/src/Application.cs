using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using Core.utils;
using Core.Data;

namespace Core.src
{
	/* a Singleton class to manage runtime application */
	public class Application
	{
		/* singleton */
		private Application() { } 
		private static Application singleton;
		public static Application getSingleton() {
			if (singleton == null) singleton = new Application();
			return singleton;
		}

		/* attribute */
		private XmlFile<ProjectData> project_file = new XmlFile<ProjectData>();

		/// <summary>
		/// initialize the application each time it starts
		/// </summary>
		public void initialize()
		{
			if (!Directory.Exists(Paths.PROGRAMME_DATA)) Directory.CreateDirectory(Paths.PROGRAMME_DATA);
			if (!Directory.Exists(Paths.LOGS)) Directory.CreateDirectory(Paths.LOGS);
		}

		/// <summary>
		/// search for project file and build project_data <para/>
		/// if project file not found throws <c>NotFoundError</c>
		/// </summary>
		public void loadProjectFile() {
			foreach ( string file in Directory.GetFiles( Directory.GetCurrentDirectory() )) {
				if (file.EndsWith(Reference.PROJECT_FILE_EXTENSION)) {
					project_file.setPath(file);
					project_file.load();
					return;
				}
			}
			throw new NotFoundError("project file not found");
		}

		public ProjectData getProjectData() {
			return this.project_file.getData();
		}

	}
}
