using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Collections.ObjectModel;

using Core.Utils;
using Core.Data.Files;
using Core.Data.Models;


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
			// programe_data_file.getData().cleanProjectPaths();
			// programe_data_file.save(); 
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
		public void createNewProject(string path, ProjectModel project_model ) {
			string project_name = project_model.name.value;
			path = Path.GetFullPath(path);

			ProjectData project_data = new ProjectData(project_name);
			project_data.location = project_model.location.value;
			project_data.client_nic = project_model.client.value.nic.value;
			project_data.creation_date = project_model.creation_date.value;
			project_data.date = project_model.date.value;
			ProjectManager.getSingleton().createProjectTemplate(path, project_name);
			var file_path = Path.Combine(path, project_name, project_name + Reference.PROJECT_FILE_EXTENSION);
			project_file.setPath( file_path );
			project_file.setData(project_data);
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

		// TODO: DANGER changes in client must reflect in database
		public ObservableCollection<ClientModel> getClients() => clients_file.getData().clients;
		public ObservableCollection<ClientModel> getClientsDropDownList() {
			ObservableCollection<ClientModel> ret = new ObservableCollection<ClientModel>() { new ClientModel("<create new client>") };
			foreach (var _client in clients_file.getData().clients) ret.Add(_client);
			return ret;
		}

		

			public void addClient( ClientModel client ) {
			clients_file.getData().clients.Add(client);
			clients_file.save();
		}
	}
}
