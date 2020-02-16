using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml.Serialization;
using System.Security.AccessControl;

namespace Core
{
	/* file tree items */
	public class FileTreeItem
	{
		[XmlAttribute] public string path;
		[XmlAttribute] public string name;
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
		public List<FileTreeItem> items = new List<FileTreeItem>();

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
		public class Dirs
		{
			public static readonly string CLIENT = "Client";
			public static readonly string SUPPLIER = "Supplier";

			public static readonly string INQUIRY_SHEET = "Inquiry Sheet";
			public static readonly string QUOTATION = "Quotation";
			public static readonly string SALES_AGREEMENT = "Sales Agreement";
			public static readonly string PROJECT_TRACKING = "Project Tracking";
			public static readonly string HANDOVER = "Handover";
			public static readonly string MAINTENANCE = "Maintenance";
		}

		private static readonly List<DirectoryItem> PROJECT_TEMPLATE = getProjectTemplate();
		public static List<DirectoryItem> getProjectTemplate() {
			return new List<DirectoryItem>() {
				new DirectoryItem( Dirs.INQUIRY_SHEET    ) .addDir(Dirs.CLIENT).addDir(Dirs.SUPPLIER),
				new DirectoryItem( Dirs.QUOTATION        ) .addDir(Dirs.CLIENT).addDir(Dirs.SUPPLIER),
				new DirectoryItem( Dirs.SALES_AGREEMENT  ) .addDir(Dirs.CLIENT).addDir(Dirs.SUPPLIER),
				new DirectoryItem( Dirs.PROJECT_TRACKING ),
				new DirectoryItem( Dirs.HANDOVER         ),
				new DirectoryItem( Dirs.MAINTENANCE      )
			};
		}

		private static ProjectManager singleton;
		private ProjectManager() { }
		public static ProjectManager getSingleton() {
			if (singleton == null) singleton = new ProjectManager();
			return singleton;
		}

		public void createProjectTemplate(string path, string project_name) {
			if (!Directory.Exists(path)) throw new InvalidPathError();
			string project_dir = Path.Combine(path, project_name); // TODO: project_name validation - throws illegal characters in path
			if (Directory.Exists(project_dir)) throw new AlreadyExistsError("project directory already exists");
			Directory.CreateDirectory(project_dir);
			foreach (DirectoryItem dir in PROJECT_TEMPLATE) buildRecursiveDirectory(project_dir, dir);
		}

		/***** PRIVATE *****/
		void buildRecursiveDirectory(string path, DirectoryItem directory) {
			Directory.CreateDirectory( Path.Combine( path, directory.path ));
			foreach ( FileTreeItem item in directory.items) {
				if (item is DirectoryItem) buildRecursiveDirectory(path, (DirectoryItem)item); // else item is file : cant' create
			}
		}
	}
}
