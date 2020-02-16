using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using Core.Utils;
using Core.Data.Files;

namespace Core
{
	/* a Singleton class to manage runtime application */
	public class Application
	{
		/* attribute */
		static Logger logger								= new Logger();
		private XmlFile<ProjectData>  project_file			= new XmlFile<ProjectData>();
		private XmlFile<ProgrameData> programe_data_file	= new XmlFile<ProgrameData>( file_path: Paths.PROGRAME_DATA_FILE );
		private BinFile<ClientsData>  clients_file			= new BinFile<ClientsData>(  file_path: Paths.CLIENTS_DATA_FILE  );

		/* singleton */
		private Application() { } 
		private static Application singleton;
		public static Application getSingleton() {
			if (singleton == null) singleton = new Application();
			return singleton;
		}


		/// <summary>
		/// initialize the application each time it starts
		/// </summary>
		public void initialize()
		{
			if (!Directory.Exists(Paths.PROGRAMME_DATA)) Directory.CreateDirectory(Paths.PROGRAMME_DATA);
			if (!Directory.Exists(Paths.SEBE_CLIENT)) Directory.CreateDirectory(Paths.SEBE_CLIENT);
			if (!Directory.Exists(Paths.LOGS)) Directory.CreateDirectory(Paths.LOGS);
			if (!File.Exists( Paths.PROGRAME_DATA_FILE)) { programe_data_file.setData(new ProgrameData()); programe_data_file.save();  }
			else { programe_data_file.load(); }
			if (!File.Exists(Paths.CLIENTS_DATA_FILE)) { clients_file.setData(new ClientsData()); clients_file.save(); }
			else { clients_file.load(); }
		}

		// public void loadProjectFile() {
		// 	foreach ( string file in Directory.GetFiles( Directory.GetCurrentDirectory() )) {
		// 		if (file.EndsWith(Reference.PROJECT_FILE_EXTENSION)) {
		// 			project_file.setPath(file);
		// 			project_file.load();
		// 			return;
		// 		}
		// 	}
		// 	logger.logError("project file not found, current directory : " + Directory.GetCurrentDirectory());
		// 	throw new FileNotFoundException("project file not found, current directory : " + Directory.GetCurrentDirectory() );
		// }

		// throws if path, name is invalid, project directory already exists
		public void createNewProject(string path, string project_name ) {
			path = Path.GetFullPath(path);
			ProjectManager.getSingleton().createProjectTemplate(path, project_name);
			var file_path = Path.Combine(path, project_name, project_name + Reference.PROJECT_FILE_EXTENSION);
			project_file.setPath( file_path );
			project_file.setData(new ProjectData( project_name ));
			project_file.save();
			programe_data_file.getData().addProjectPath(file_path);
			programe_data_file.save();
			Directory.SetCurrentDirectory(Path.Combine(path, project_name));
		}

		public void loadProject(string path) {
			path = Path.GetFullPath(path);
			project_file.setPath(path);
			project_file.load();
			Directory.SetCurrentDirectory( path );
		}

		public ProjectData getProjectData() {
			return this.project_file.getData();
		}

		public List<string> getRecentProjects() => programe_data_file.getData()._recent_projects;
		public void setRecentProject(int index) => programe_data_file.getData().setMostRecentProject(index);
		
	}
}
