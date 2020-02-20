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
		static Logger logger = new Logger();
		public XmlFile<ProgrameData> programe_data_file	{ get; } = new XmlFile<ProgrameData>( file_path: Paths.PROGRAME_DATA_FILE );
		public BinFile<ClientsData> clients_file		{ get; } = new BinFile<ClientsData>(file_path: Paths.CLIENTS_DATA_FILE);

		private ObservableCollection<ClientModel> _dropdown_clients_list = null;

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
			ProjectManager.initialize();

			if (!Directory.Exists(Paths.PROGRAMME_DATA)) Directory.CreateDirectory(Paths.PROGRAMME_DATA);
			if (!Directory.Exists(Paths.DEFAULT_PROJ_DIR)) Directory.CreateDirectory(Paths.DEFAULT_PROJ_DIR);
			if (!Directory.Exists(Paths.SEBE_CLIENT)) Directory.CreateDirectory(Paths.SEBE_CLIENT);
			if (!Directory.Exists(Paths.LOGS)) Directory.CreateDirectory(Paths.LOGS);
			if (!File.Exists( Paths.PROGRAME_DATA_FILE)) { programe_data_file.data = new ProgrameData(); programe_data_file.save();  }
			else { programe_data_file.load(); }
			if (!File.Exists(Paths.CLIENTS_DATA_FILE)) { clients_file.data = new ClientsData(); clients_file.save(); }
			else { clients_file.load(); }

			programe_data_file.data.cleanProjectPaths();
			programe_data_file.save(); 
		}

		// throws if path, name is invalid, project directory already exists

		public void createNewProject(ProjectModel project_model, string path = null ) {
			if (path is null) path = programe_data_file.data.default_proj_dir;
			path = Path.GetFullPath(path);
			ProjectManager.singleton.createProjectTemplate(path, project_model);
			programe_data_file.data.addProjectPath(ProjectManager.singleton.project_file.path);
			programe_data_file.save();
			Directory.SetCurrentDirectory(Path.Combine(path, project_model.name.value));
		}

		public void loadProject(string path) {
			path = Path.GetFullPath(path);
			ProjectManager.singleton.loadProject(path);
			Directory.SetCurrentDirectory( Path.GetDirectoryName(path) );
		}

		public List<string> getRecentProjects() => programe_data_file.data._recent_projects;
		public void setRecentProject(int index) => programe_data_file.data.setMostRecentProject(index);
		public void setDefaultProjectPath(string path) {
			if (!Directory.Exists(path)) throw new DirectoryNotFoundException("default project path directory not exists");
			programe_data_file.data.default_proj_dir = path;
			programe_data_file.save();
		}
		// TODO: DANGER changes in client must reflect in database
		public ObservableCollection<ClientModel> getClients() => clients_file.data.clients;
		public ObservableCollection<ClientModel> getClientsDropDownList() {
			if (_dropdown_clients_list is null) {
				_dropdown_clients_list = new ObservableCollection<ClientModel>() { new ClientModel("<create new client>") };
				foreach (var _client in clients_file.data.clients) _dropdown_clients_list.Add(_client);
			}
			return _dropdown_clients_list;
		}

		public void addClient( ClientModel client ) {
			clients_file.data.clients.Add(client);
			clients_file.save();
			//throw new Exception("TODO: cache to upload");
		}
	}
}
