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
using System.Reflection;
using Core.Data;

namespace Core
{
	/* a Singleton class to manage runtime application */
	public class Application
	{
		/* attribute */
		static Logger logger = new Logger();
		public XmlFile<ProgrameData> programe_data_file	{ get; } = new XmlFile<ProgrameData>( file_path: Paths.PROGRAME_DATA_FILE );
		public BinFile<ClientsData> clients_file		{ get; } = new BinFile<ClientsData>(file_path: Paths.CLIENTS_DATA_FILE);
		public BinFile<SuppliersData> suppliers_file	{ get; } = new BinFile<SuppliersData>(file_path: Paths.SUPPLIERS_DATA_FILE);

		private bool _is_proj_loaded = false;
		public bool is_proj_loaded { get { return _is_proj_loaded; } }

		private List<ClientModel> _dropdown_clients_list = null;

		/* singleton */
		private Application() { } 
		private static Application _singleton;
		public static Application singleton {
			get {
				if (_singleton == null) _singleton = new Application();
				return _singleton;
			}
		}
		[Obsolete("this method will dipricate, use Application.singleton property", false)]
		public static Application getSingleton() {
			if (_singleton == null) _singleton = new Application();
			return _singleton;
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
			if (!File.Exists(Paths.SUPPLIERS_DATA_FILE)) { suppliers_file.data = new SuppliersData(); suppliers_file.save(); }
			else { suppliers_file.load(); }
			if (!Directory.Exists(Paths.UPLOAD_CACHE)) Directory.CreateDirectory(Paths.UPLOAD_CACHE);

			programe_data_file.data.cleanProjectPaths();
			programe_data_file.save(); 
		}

		// throws if path, name is invalid, project directory already exists
		public void createNewProject(ModelAPI<ProjectModel> api, string path = null ) {
			if (api.api_mode != ModelApiMode.MODE_CREATE) throw new InvalidOperationException("api mode must be MODE_CREATE");
			if (path is null) path = programe_data_file.data.default_proj_dir;
			path = Path.GetFullPath(path);
			api.model.id.value = DateTime.Now.Ticks; // TODO: consider change in pk
			ProjectManager.singleton.createProjectTemplate(path, api);
			var client = Model.getModel(api.model.client_id.value, ModelType.MODEL_CLIENT) as ClientModel;
			programe_data_file.data.addProject(api.model.name.value, client.name.value, ProjectManager.singleton.project_file.path);
			programe_data_file.save();
			Directory.SetCurrentDirectory(Path.Combine(path, api.model.name.value));
			_is_proj_loaded = true;
		}
		public XmlFile<ProjectData> _addExistingProject(string path) {
			if (!File.Exists(path)) throw new FileNotFoundException("invalid file path : " + path);
			// !path.EndsWith(Core.Reference.PROJECT_FILE_EXTENSION
			XmlFile<ProjectData> proj = new XmlFile<ProjectData>(file_path: path);
			proj.load();
			var client = Model.getModel(proj.data.project_model.client_id.value, ModelType.MODEL_CLIENT) as ClientModel;
			programe_data_file.data.addProject(proj.data.project_model.name.value, client.name.value, proj.path);
			programe_data_file.save();
			return proj;
		}
		public void addAndLoadExistingProject(string path) {
			var proj = _addExistingProject(path);
			ProjectManager.singleton.project_file.data = proj.data;
			Directory.SetCurrentDirectory( Path.GetDirectoryName(path) );
		}

		public void loadProject(int index) {
			var recent_projects = programe_data_file.data.recent_projects;
			if (recent_projects.Count == 0) throw new IndexOutOfRangeException("no projects available");
			var proj = recent_projects[index]; 
			string path = proj.path;
			path = Path.GetFullPath(path);
			ProjectManager.singleton.loadProject(path);
			programe_data_file.data.setMostRecentProject(index);
			programe_data_file.save();
			Directory.SetCurrentDirectory( Path.GetDirectoryName(path) );
			_is_proj_loaded = true;
		}


		public List<ProgrameData.ProjectViewData> getRecentProjects() => programe_data_file.data.recent_projects;
		public void setRecentProject(int index) => programe_data_file.data.setMostRecentProject(index);
		public void setDefaultProjectPath(string path) {
			if (!Directory.Exists(path)) throw new DirectoryNotFoundException("default project path directory not exists");
			programe_data_file.data.default_proj_dir = path;
			programe_data_file.save();
		}

		public List<ClientModel> getClients() => clients_file.data.clients;
		public List<ClientModel> getClientsDropDownList() {
			if (_dropdown_clients_list is null) {
				var create_new_client = new ClientModel("<create new client>");
				create_new_client.id.value = 0;
				_dropdown_clients_list = new List<ClientModel>() { create_new_client };
				foreach (var _client in clients_file.data.clients) _dropdown_clients_list.Add(_client);
			}
			return _dropdown_clients_list;
		}
	}
}
