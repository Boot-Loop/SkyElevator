using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Core.src
{
	/* file tree items */
	public class FileTreeItem
	{
		public string path, name;
		public FileTreeItem(string path) {
			this.path = path;
			this.name = Path.GetFileName(this.path);
		}
	}
	public class FileItem : FileTreeItem 
	{ 
		public FileItem(string path) : base(path) { } 
	}
	public class DirectoryItem : FileTreeItem
	{
		public List<FileTreeItem> items = new List<FileTreeItem>();
		public DirectoryItem(string path) : base(path) { }
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

		
	}
	/***********************/

	public class ProjectManager
	{
		static readonly string CLIENT	= "Client";
		static readonly string SUPPLIER	= "Supplier";

		static readonly string INQUIRY_SHEET		= "Inquiry Sheet";
		static readonly string QUOTATION			= "Quotation";
		static readonly string SALES_AGREEMENT		= "Sales Agreement";
		static readonly string PROJECT_TRACKING		= "Project Tracking";
		static readonly string HANDOVER				= "Handover";
		static readonly string MAINTENANCE			= "Maintenance";

		public static readonly List<DirectoryItem> PROJECT_TEMPLATE = new List<DirectoryItem>()
		{
			new DirectoryItem( INQUIRY_SHEET	),
			new DirectoryItem( QUOTATION		) .addDir(CLIENT).addDir(SUPPLIER) ,
			new DirectoryItem( SALES_AGREEMENT	) .addDir(CLIENT).addDir(SUPPLIER),
			new DirectoryItem( PROJECT_TRACKING	),
			new DirectoryItem( HANDOVER			),
			new DirectoryItem( MAINTENANCE      ),
		};

		private static ProjectManager singleton;
		private ProjectManager() { }
		public static ProjectManager getSingleton() {
			if (singleton == null) singleton = new ProjectManager();
			return singleton;
		}

		public void createNewProject(string project_name, string path) {
			if (!Directory.Exists(path)) throw new InvalidPathError();
			string project_dir = Path.Combine(path, project_name); // TODO: project_name validation - throws illegal characters in path
			if (Directory.Exists(project_dir)) throw new AlreadyExistsError("project directory already exists");
			Directory.CreateDirectory(project_dir);
			foreach (DirectoryItem dir in PROJECT_TEMPLATE) buildRecursiveFileTree(project_dir, dir);
		}

		/***** PRIVATE *****/
		void buildRecursiveFileTree(string path, DirectoryItem directory) {
			Directory.CreateDirectory( Path.Combine( path, directory.path ));
			foreach ( FileTreeItem item in directory.items) {
				if (item is DirectoryItem) buildRecursiveFileTree(path, (DirectoryItem)item); // else item is file : cant' create
			}
		}
	}
}
