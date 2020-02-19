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
			if (!Directory.Exists(Paths.DEFAULT_PROJ_DIR)) Directory.CreateDirectory(Paths.DEFAULT_PROJ_DIR);
			if (!Directory.Exists(Paths.SEBE_CLIENT)) Directory.CreateDirectory(Paths.SEBE_CLIENT);
			if (!Directory.Exists(Paths.LOGS)) Directory.CreateDirectory(Paths.LOGS);
			if (!File.Exists( Paths.PROGRAME_DATA_FILE)) { programe_data_file.data = new ProgrameData(); programe_data_file.save();  }
			else { programe_data_file.load(); }
			if (!File.Exists(Paths.CLIENTS_DATA_FILE)) { clients_file.setData(new ClientsData()); clients_file.save(); }
			else { clients_file.load(); }
		}

		// throws if path, name is invalid, project directory already exists
		public void createNewProject(string project_name, string path = null ) {
			if (path is null) path = programe_data_file.data.default_proj_dir;
			path = Path.GetFullPath(path);
			ProjectManager.singleton.createProjectTemplate(path, project_name);
			programe_data_file.data.addProjectPath(ProjectManager.singleton.project_file.path);
			programe_data_file.save();
			Directory.SetCurrentDirectory(Path.Combine(path, project_name));
		}

		public void loadProject(string path) {
			path = Path.GetFullPath(path);
			ProjectManager.singleton.loadProject(path);
			Directory.SetCurrentDirectory( Path.GetDirectoryName(path) );
		}

		public List<string> getRecentProjects() => programe_data_file.data._recent_projects;
		public void setRecentProject(int index) => programe_data_file.data.setMostRecentProject(index);

		// TODO: DANGER changes in client must reflect in database
		public ObservableCollection<ClientModel> getClients() => clients_file.getData().clients;
		public void addClient( ClientModel client ) {
			clients_file.getData().clients.Add(client);
			clients_file.save();
		}
	}
}
