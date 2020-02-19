﻿using System;
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
			public static readonly string DOT_SKY_DIR		= ".sky";
			public static readonly string DRAFTS			= "drafts";
			public static readonly string SEBE_CACHE		= "sebe_cache";

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
		}

		public XmlFile<ProjectData>				project_file		{ get; set; } = new XmlFile<ProjectData>();
		public XmlFile<ClientProgressModel>		progress_client		{ get; set; } = new XmlFile<ClientProgressModel>();
		public XmlFile<SupplierProgressModel>	progress_supplier	{ get; set; } = new XmlFile<SupplierProgressModel>();

		private static ProjectManager _singleton;
		public static ProjectManager singleton {
			get { 
				if (_singleton == null) _singleton = new ProjectManager();
				return _singleton; 
			}
		}
		private ProjectManager() { }

		private static readonly List<FileTreeItem> PROJECT_TEMPLATE = getProjectTemplate();
		public static List<FileTreeItem> getProjectTemplate() {
			return new List<FileTreeItem>() {
				new DirectoryItem( Dirs.DOT_SKY_DIR		 ) .addDir(Dirs.DRAFTS) .addDir(Dirs.SEBE_CACHE),
				new DirectoryItem( Dirs.INQUIRY_SHEET    ) .addDir(Dirs.CLIENT) .addDir(Dirs.SUPPLIER)	,
				new DirectoryItem( Dirs.QUOTATION        ) .addDir(Dirs.CLIENT) .addDir(Dirs.SUPPLIER)	,
				new DirectoryItem( Dirs.SALES_AGREEMENT  ) .addDir(Dirs.CLIENT) .addDir(Dirs.SUPPLIER)	,
				new DirectoryItem( Dirs.PROJECT_TRACKING ),
				new DirectoryItem( Dirs.HANDOVER         ),
				new DirectoryItem( Dirs.MAINTENANCE      ),
			};
		}


		public void createProjectTemplate(string path, string project_name) {
			if (!Directory.Exists(path)) throw new InvalidPathError();
			string project_dir = Path.Combine(path, project_name); // TODO: project_name validation - throws illegal characters in path
			if (Directory.Exists(project_dir)) throw new AlreadyExistsError("project directory already exists");
			Directory.CreateDirectory(project_dir);
			foreach (FileTreeItem item in PROJECT_TEMPLATE) buildRecursiveDirectory(project_dir, item);

			// project file
			var proj_file_path  = Path.Combine(path, project_name, project_name + Reference.PROJECT_FILE_EXTENSION);
			project_file.path	= proj_file_path;
			project_file.data	= new ProjectData(project_name);
			project_file.data.dirs.addFile(project_name + Reference.PROJECT_FILE_EXTENSION);

			// progress client
			progress_client.path = Path.Combine(path, project_name, Dirs.PROJECT_TRACKING, Files.PROGRESS_CLIENT);
			progress_client.data = new ClientProgressModel();
			progress_client.save();
			project_file.data.dirs.getDir(Dirs.PROJECT_TRACKING).addFile(Files.PROGRESS_CLIENT);

			// progress supplier
			progress_supplier.path = Path.Combine(path, project_name, Dirs.PROJECT_TRACKING, Files.PROGRESS_SUPPLIER);
			progress_supplier.data = new SupplierProgressModel();
			progress_supplier.save();
			project_file.data.dirs.getDir(Dirs.PROJECT_TRACKING).addFile(Files.PROGRESS_SUPPLIER);

			project_file.save();
		}

		public void loadProject(string path) {
			project_file.path = path;
			project_file.load();

			path = Path.GetDirectoryName(path);

			var progress_client_path = Path.Combine(path, project_file.data.dirs.getDir(Dirs.PROJECT_TRACKING).getFile(Files.PROGRESS_CLIENT).path);
			progress_client.path = progress_client_path;
			progress_client.load();

			var progress_supplier_path = Path.Combine(path, project_file.data.dirs.getDir(Dirs.PROJECT_TRACKING).getFile(Files.PROGRESS_SUPPLIER).path);
			progress_supplier.path = progress_supplier_path;
			progress_supplier.load();
		}

		/***** PRIVATE *****/
		void buildRecursiveDirectory(string path, FileTreeItem item) {
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
	}
}
