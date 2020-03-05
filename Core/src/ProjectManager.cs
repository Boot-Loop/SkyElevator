using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml.Serialization;
using System.Security.AccessControl;

using Core.Data.Files;
using Core.Data.Models;
using Core.Utils;
using Core.Data;

namespace Core
{
	/* file tree items */
	public class FileTreeItem
	{
		[XmlAttribute] public string path { get; set; }
		[XmlAttribute] public string name { get; set; }
		private FileTreeItem() { this.name = ""; this.path = ""; }
		public FileTreeItem(string path) {
			this.path = path;
			this.name = Path.GetFileName(this.path);
		}
	}
	public class FileItem : FileTreeItem 
	{ 
		private FileItem() : base("") { }
		public FileItem(string path) : base(path) { } 
	}
	public class DirectoryItem : FileTreeItem
	{
		[XmlArray("items")]
		[XmlArrayItem("file", typeof(FileItem))]
		[XmlArrayItem("directory", typeof(DirectoryItem))]
		public List<FileTreeItem> items { get; set; } = new List<FileTreeItem>();

		private DirectoryItem() : base("") { }
		public DirectoryItem(string path="") : base(path) { }

		// TODO: if add 2 files / dirs with the same name consider throwing an error

		public DirectoryItem addFile(string name) { 
			this.items.Add(new FileItem( Path.Combine( this.path, name)));  return this; 
		}
		public DirectoryItem addFile(FileItem file) { 
			this.items.Add( file );  return this; 
		}

		public DirectoryItem addDir(string name) { 
			this.items.Add(new DirectoryItem( Path.Combine(this.path, name) ));  return this; 
		}
		public DirectoryItem addDir(DirectoryItem dir) { 
			this.items.Add( dir );  return this; 
		}

		public FileTreeItem getItem(string name) {
			foreach (FileTreeItem item in items) {
				if (item.name == name) return item;
			}
			return null;
		}
		public DirectoryItem getDir(string name) {
			FileTreeItem item = getItem(name);
			if (item is DirectoryItem) return (DirectoryItem)item;
			return null;
		}
		public FileItem getFile(string name) {
			FileTreeItem item = getItem(name);
			if (item is FileItem) return (FileItem)item;
			return null;
		}
		
	}
	/***********************/

	public class ProjectManager
	{
		public enum ProjectType
		{
			INSTALLATION,
			MAINTENANCE,
			REPAIR_OR_MODERNIZATION,
			OTHERS,
		}
		private static Dictionary<ProjectType, bool> has_progress_tracking = null;

		public class Dirs
		{
			public static readonly string DOT_SKY_DIR		= ".sky";
			public static readonly string DRAFTS			= "drafts";
			//public static readonly string SEBE_CACHE		= "sebe_cache";

			public static readonly string CLIENT			= "Client";
			public static readonly string SUPPLIER			= "Supplier";

			public static readonly string INQUIRY_SHEET		= "Inquiry Sheet";
			public static readonly string QUOTATION			= "Quotation";
			public static readonly string SALES_AGREEMENT	= "Sales Agreement";
			public static readonly string PROJECT_TRACKING	= "Project Tracking";
			public static readonly string HANDOVER			= "Handover";
			public static readonly string MAINTENANCE		= "Maintenance";
		}
		public class Files
		{
			public static readonly string PROGRESS_CLIENT	= "progress_client.xml";
			public static readonly string PROGRESS_SUPPLIER = "progress_supplier.xml";
			public static readonly string PROGRESS_PAYMENTS = "progress_payments.xml";
		}

		public XmlFile<ProjectData>				project_file		{ get; } = new XmlFile<ProjectData>();
		public XmlFile<ClientProgressModel>		progress_client		{ get; } = new XmlFile<ClientProgressModel>();
		public XmlFile<SupplierProgressModel>	progress_supplier	{ get; } = new XmlFile<SupplierProgressModel>();
		public XmlFile<List<PaymentModel>>		progress_payments	{ get; } = new XmlFile<List<PaymentModel>>(data:new List<PaymentModel>());


		private static ProjectManager _singleton;
		public static ProjectManager singleton {
			get { 
				if (_singleton == null) _singleton = new ProjectManager();
				return _singleton; 
			}
		}
		private ProjectManager() { }

		public static void initialize() {
			if (has_progress_tracking is null) {
				has_progress_tracking = new Dictionary<ProjectType, bool>();
				foreach (var type in Enum.GetValues(typeof(ProjectType))) {
					has_progress_tracking[(ProjectType)type] = hasProgressTracking((ProjectType)type);
				}
			}
		}

		public static List<FileTreeItem> getProjectTemplate(ProjectType project_type = ProjectType.INSTALLATION) {
			switch (project_type)
			{
				case ProjectType.INSTALLATION:
					{
						return new List<FileTreeItem>() {
							new DirectoryItem( Dirs.DOT_SKY_DIR		 ) .addDir(Dirs.DRAFTS), //.addDir(Dirs.SEBE_CACHE),
							new DirectoryItem( Dirs.INQUIRY_SHEET    ) .addDir(Dirs.CLIENT) .addDir(Dirs.SUPPLIER)	,
							new DirectoryItem( Dirs.QUOTATION        ) .addDir(Dirs.CLIENT) .addDir(Dirs.SUPPLIER)	,
							new DirectoryItem( Dirs.SALES_AGREEMENT  ) .addDir(Dirs.CLIENT) .addDir(Dirs.SUPPLIER)	,
							new DirectoryItem( Dirs.PROJECT_TRACKING ),
							new DirectoryItem( Dirs.HANDOVER         ),
							new DirectoryItem( Dirs.MAINTENANCE      ),
						};
					}
				case ProjectType.MAINTENANCE:
					{
						return new List<FileTreeItem>() {
							new DirectoryItem( Dirs.DOT_SKY_DIR      ) .addDir(Dirs.DRAFTS), //.addDir(Dirs.SEBE_CACHE),
							new DirectoryItem( Dirs.QUOTATION        ) .addDir(Dirs.CLIENT) .addDir(Dirs.SUPPLIER)  ,
							new DirectoryItem( Dirs.SALES_AGREEMENT  ) .addDir(Dirs.CLIENT) .addDir(Dirs.SUPPLIER)  ,
							new DirectoryItem( Dirs.HANDOVER         ),
							new DirectoryItem( Dirs.MAINTENANCE      ),
						};
					}
				case ProjectType.REPAIR_OR_MODERNIZATION:
					{
						return new List<FileTreeItem>() {
							new DirectoryItem( Dirs.DOT_SKY_DIR      ) .addDir(Dirs.DRAFTS), //.addDir(Dirs.SEBE_CACHE),
							new DirectoryItem( Dirs.QUOTATION        ) .addDir(Dirs.CLIENT) .addDir(Dirs.SUPPLIER)  ,
							new DirectoryItem( Dirs.SALES_AGREEMENT  ) .addDir(Dirs.CLIENT) .addDir(Dirs.SUPPLIER)  ,
							new DirectoryItem( Dirs.PROJECT_TRACKING ),
							new DirectoryItem( Dirs.HANDOVER         ),
							new DirectoryItem( Dirs.MAINTENANCE      ),
						};
					}
				default: // case others
					{
						return new List<FileTreeItem>() {
							new DirectoryItem( Dirs.DOT_SKY_DIR      ) .addDir(Dirs.DRAFTS), //.addDir(Dirs.SEBE_CACHE),
							new DirectoryItem( Dirs.INQUIRY_SHEET    ) .addDir(Dirs.CLIENT) .addDir(Dirs.SUPPLIER)  ,
							new DirectoryItem( Dirs.QUOTATION        ) .addDir(Dirs.CLIENT) .addDir(Dirs.SUPPLIER)  ,
							new DirectoryItem( Dirs.SALES_AGREEMENT  ) .addDir(Dirs.CLIENT) .addDir(Dirs.SUPPLIER)  ,
							new DirectoryItem( Dirs.PROJECT_TRACKING ),
							new DirectoryItem( Dirs.HANDOVER         ),
							new DirectoryItem( Dirs.MAINTENANCE      ),
						};
					}
			}
		}

		public bool hasProgressTracking() {
			if (project_file.data is null) throw new NullReferenceException("project data is null"); 
			return has_progress_tracking[ project_file.data.project_model.getProjectType() ];
		}


		public void createProjectTemplate(string path, ModelAPI<ProjectModel> api ) {
			var project_model = api.model;
			string project_name = project_model.name.value;
			if (!Directory.Exists(path)) Logger.logThrow( new DirectoryNotFoundException() );
			string project_dir = Path.Combine(path, project_name); // TODO: project_name validation - throws illegal characters in path
			if (Directory.Exists(project_dir)) Logger.logThrow( new AlreadyExistsError("project directory already exists"));
			Directory.CreateDirectory(project_dir);
			foreach (FileTreeItem item in getProjectTemplate(project_model.getProjectType())) buildRecursiveDirectory(project_dir, item);

			if (project_model.client_id.isNull()) throw new Exception("client must not be null for a project");

			// project file
			var proj_file_path  = Path.Combine(path, project_name, project_name + Reference.PROJECT_FILE_EXTENSION);
			project_file.path	= proj_file_path;
			// project_file.data.project_model = project_model;
			api.update(); // creates project file and save, upload cache

			var file_items = getProjectTemplate(project_model.getProjectType());
			if (has_progress_tracking is null) throw new NullReferenceException("did you call Application.singleton.initialize()");
			if (has_progress_tracking[project_model.getProjectType()]) {

				var progress_pk = DateTime.Now.Ticks;

				// progress client
				progress_client.path = Path.Combine(path, project_name, Dirs.PROJECT_TRACKING, Files.PROGRESS_CLIENT);
				ModelAPI<ClientProgressModel> api_progress_client = new ModelAPI<ClientProgressModel>(null, ModelApiMode.MODE_CREATE);
				api_progress_client.model.id.value = progress_pk;
				api_progress_client.model.project_id.value = api.model.id.value;
				api_progress_client.update(); // create and save progress_client file
				project_file.data.dirs.getDir(Dirs.PROJECT_TRACKING).addFile(Files.PROGRESS_CLIENT);

				// progress supplier
				progress_supplier.path = Path.Combine(path, project_name, Dirs.PROJECT_TRACKING, Files.PROGRESS_SUPPLIER);
				ModelAPI<SupplierProgressModel> api_progress_supplier = new ModelAPI<SupplierProgressModel>(null, ModelApiMode.MODE_CREATE);
				api_progress_supplier.model.id.value = progress_pk;
				api_progress_supplier.model.project_id.value = api.model.id.value;
				api_progress_supplier.update(); // create and save progress_supplier file
				project_file.data.dirs.getDir(Dirs.PROJECT_TRACKING).addFile(Files.PROGRESS_SUPPLIER);

				// payments
				progress_payments.path = Path.Combine(path, project_name, Dirs.PROJECT_TRACKING, Files.PROGRESS_PAYMENTS);
				progress_payments.save();
				project_file.data.dirs.getDir(Dirs.PROJECT_TRACKING).addFile(Files.PROGRESS_PAYMENTS);
			}

			project_file.save();
		}

		public void loadProject(string path) {
			project_file.path = path;
			project_file.load();
			if (has_progress_tracking is null) Logger.logThrow(new NullReferenceException("did you call Application.singleton.initialize()"));
			if (has_progress_tracking[project_file.data.project_model.getProjectType()]) {
				path = Path.GetDirectoryName(path);

				var progress_client_path = Path.Combine(path, project_file.data.dirs.getDir(Dirs.PROJECT_TRACKING).getFile(Files.PROGRESS_CLIENT).path);
				progress_client.path = progress_client_path;
				progress_client.load();

				var progress_supplier_path = Path.Combine(path, project_file.data.dirs.getDir(Dirs.PROJECT_TRACKING).getFile(Files.PROGRESS_SUPPLIER).path);
				progress_supplier.path = progress_supplier_path;
				progress_supplier.load();

				var progress_payments_path = Path.Combine(path, project_file.data.dirs.getDir(Dirs.PROJECT_TRACKING).getFile(Files.PROGRESS_PAYMENTS).path);
				progress_payments.path = progress_payments_path;
				progress_payments.load();
			}

		}

		/***** PRIVATE *****/
		private static void buildRecursiveDirectory(string path, FileTreeItem item) {
			if (item is DirectoryItem) {
				var directory = (DirectoryItem)item;
				DirectoryInfo dir_info = Directory.CreateDirectory(Path.Combine(path, directory.path));
				if (directory.name == Dirs.DOT_SKY_DIR) { dir_info.Attributes |= FileAttributes.Hidden; }
				foreach (FileTreeItem _item in directory.items) {
					buildRecursiveDirectory(path, _item);
				}
			}
			else if (item is FileItem) { } // do nothing
		}

		private static bool hasProgressTracking(ProjectType type) {
			var file_items = getProjectTemplate(type);
			foreach (var item in file_items) {
				if (item is DirectoryItem) {
					if (((DirectoryItem)item).name == Dirs.PROJECT_TRACKING)
						return true;
				}
			}
			return false;
		}

	}
}
