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

	public class ProjectManager
	{
		public enum ProjectType
		{
			INSTALLATION			= 1,
			MAINTENANCE				= 2,
			REPAIR_OR_MODERNIZATION = 3,
			OTHERS					= 4,
		}

		public class Dirs
		{
			public static readonly string DOT_SKY_DIR				= ".sky";
			public static readonly string DRAFTS					= Path.Combine(DOT_SKY_DIR, "drafts");
			public static readonly string CLIENT					= "Client";
			public static readonly string SUPPLIER					= "Supplier";

			public static readonly string INQUIRY_SHEET				= "Inquiry Sheet";
			public static readonly string INQUIRY_SHEET_CLIENT		= Path.Combine(INQUIRY_SHEET, CLIENT);
			public static readonly string INQUIRY_SHEET_SUPPLIER	= Path.Combine(INQUIRY_SHEET, SUPPLIER);
			public static readonly string QUOTATION					= "Quotation";
			public static readonly string QUOTATION_CLIENT			= Path.Combine(QUOTATION, CLIENT);
			public static readonly string QUOTATION_SUPPLIER		= Path.Combine(QUOTATION, SUPPLIER);
			public static readonly string SALES_AGREEMENT			= "Sales Agreement";
			public static readonly string SALES_AGREEMENT_CLIENT	= Path.Combine(SALES_AGREEMENT, CLIENT);
			public static readonly string SALES_AGREEMENT_SUPPLIER	= Path.Combine(SALES_AGREEMENT, SUPPLIER);
			public static readonly string PROGRESS_TRACKING			= "Progress Tracking";
			public static readonly string HANDOVER					= "Handover";
			public static readonly string MAINTENANCE				= "Maintenance";
		}
		public class Files
		{
			public static readonly string PROGRESS_CLIENT	= Path.Combine( Dirs.PROGRESS_TRACKING, "progress_client.xml");
			public static readonly string PROGRESS_SUPPLIER = Path.Combine( Dirs.PROGRESS_TRACKING, "progress_supplier.xml");
			public static readonly string PROGRESS_PAYMENTS = Path.Combine( Dirs.PROGRESS_TRACKING, "progress_payments.xml");
		}

		public XmlFile<ProjectData>				project_file		{ get; } = new XmlFile<ProjectData>();
		public XmlFile<ClientProgressModel>		progress_client		{ get; } = new XmlFile<ClientProgressModel>();
		public XmlFile<SupplierProgressModel>	progress_supplier	{ get; } = new XmlFile<SupplierProgressModel>();
		public XmlFile<List<PaymentModel>>		progress_payments	{ get; } = new XmlFile<List<PaymentModel>>(data:new List<PaymentModel>());

		public string project_dir { get {
				if (project_file.path is null) throw new NullReferenceException("project_file.path was null. did you load a project?");
				return Path.GetDirectoryName(project_file.path);  
			} }
		private  List<string> project_template = null; // set when create and load a project

		private static ProjectManager _singleton;
		public static ProjectManager singleton {
			get { 
				if (_singleton == null) _singleton = new ProjectManager();
				return _singleton; 
			}
		}
		private ProjectManager() { }

		public static void initialize() { }


		public static List<string> getProjectTemplate(ProjectType project_type = ProjectType.INSTALLATION) {
			switch (project_type)
			{
				case ProjectType.INSTALLATION:
					{
						return new List<string>() {
							Dirs.DOT_SKY_DIR, Dirs.DRAFTS,

							Dirs.INQUIRY_SHEET_CLIENT,
							Dirs.INQUIRY_SHEET_SUPPLIER,
							Dirs.QUOTATION_CLIENT,
							Dirs.QUOTATION_SUPPLIER,
							Dirs.SALES_AGREEMENT_CLIENT,
							Dirs.SALES_AGREEMENT_SUPPLIER,
							Dirs.PROGRESS_TRACKING,
							Dirs.HANDOVER,
							Dirs.MAINTENANCE,
						};
					}
				case ProjectType.MAINTENANCE:
					{
						return new List<string>() {
							Dirs.DOT_SKY_DIR, Dirs.DRAFTS,

							Dirs.QUOTATION_CLIENT,
							Dirs.QUOTATION_SUPPLIER,
							Dirs.SALES_AGREEMENT_CLIENT,
							Dirs.SALES_AGREEMENT_SUPPLIER,
							Dirs.HANDOVER,
							Dirs.MAINTENANCE,
						};
					}
				case ProjectType.REPAIR_OR_MODERNIZATION:
					{
						return new List<string>() {
							Dirs.DOT_SKY_DIR, Dirs.DRAFTS,

							Dirs.QUOTATION_CLIENT,
							Dirs.QUOTATION_SUPPLIER,
							Dirs.SALES_AGREEMENT_CLIENT,
							Dirs.SALES_AGREEMENT_SUPPLIER,
							Dirs.PROGRESS_TRACKING,
							Dirs.HANDOVER,
							Dirs.MAINTENANCE,
						};
					}
				default: // case others
					{
						return new List<string>() {
							Dirs.DOT_SKY_DIR, Dirs.DRAFTS,

							Dirs.INQUIRY_SHEET_CLIENT,
							Dirs.INQUIRY_SHEET_SUPPLIER,
							Dirs.QUOTATION_CLIENT,
							Dirs.QUOTATION_SUPPLIER,
							Dirs.SALES_AGREEMENT_CLIENT,
							Dirs.SALES_AGREEMENT_SUPPLIER,
							Dirs.PROGRESS_TRACKING,
							Dirs.HANDOVER,
							Dirs.MAINTENANCE,
						};
					}
			}
		}

		public bool hasProgressTracking() {
			if (project_file.data is null) throw new NullReferenceException("project data is null");
			return project_template.Contains(Dirs.PROGRESS_TRACKING);
		}
		public bool hasInquirySheet() {
			if (project_file.data is null) throw new NullReferenceException("project data is null");
			return project_template.Contains(Dirs.INQUIRY_SHEET_CLIENT);
		}


		public void createProjectTemplate(string path, ModelAPI<ProjectModel> api ) {
			var project_model = api.model;
			string project_name = project_model.name.value;
			if (project_model.client_id.isNull()) throw new Exception("client must not be null for a project");
			project_template = getProjectTemplate(project_model.getProjectType());

			if (!Directory.Exists(path)) throw new DirectoryNotFoundException();
			string project_dir = Path.Combine(path, project_name); // TODO: project_name validation - throws illegal characters in path
			if (Directory.Exists(project_dir)) throw new AlreadyExistsError("project directory already exists");
			Directory.CreateDirectory(project_dir);
			foreach ( string dir in getProjectTemplate(project_model.getProjectType())) {
				DirectoryInfo dir_info = Directory.CreateDirectory(Path.Combine(project_dir, dir));
				if (dir == Dirs.DOT_SKY_DIR) { dir_info.Attributes |= FileAttributes.Hidden; }
			}

			// project file
			var proj_file_path  = Path.Combine(path, project_name, project_name + Reference.PROJECT_FILE_EXTENSION);
			project_file.path	= proj_file_path;
			api.update(); // creates project file and save, upload cache

			var file_items = getProjectTemplate(project_model.getProjectType());
			if (hasProgressTracking()) {

				var progress_pk = DateTime.Now.Ticks;

				// progress client
				progress_client.path = Path.Combine(path, project_name, Files.PROGRESS_CLIENT);
				ModelAPI<ClientProgressModel> api_progress_client = new ModelAPI<ClientProgressModel>(null, ModelApiMode.MODE_CREATE);
				api_progress_client.model.id.value = progress_pk;
				api_progress_client.model.project_id.value = api.model.id.value;
				api_progress_client.update(); // create and save progress_client file
				project_file.data.items.progress_tracking.client = Files.PROGRESS_CLIENT;

				// progress supplier
				progress_supplier.path = Path.Combine(path, project_name, Files.PROGRESS_SUPPLIER);
				ModelAPI<SupplierProgressModel> api_progress_supplier = new ModelAPI<SupplierProgressModel>(null, ModelApiMode.MODE_CREATE);
				api_progress_supplier.model.id.value = progress_pk;
				api_progress_supplier.model.project_id.value = api.model.id.value;
				api_progress_supplier.update(); // create and save progress_supplier file
				project_file.data.items.progress_tracking.supplier = Files.PROGRESS_SUPPLIER;

				// payments
				progress_payments.path = Path.Combine(path, project_name, Files.PROGRESS_PAYMENTS);
				progress_payments.save();
				project_file.data.items.progress_tracking.payments = Files.PROGRESS_PAYMENTS;
			}

			project_file.save();
		}

		public void loadProject(string path) {
			project_file.path = path;
			project_file.load();
			project_template = getProjectTemplate(project_file.data.project_model.getProjectType());
			if (hasProgressTracking()) {
				path = Path.GetDirectoryName(path);

				var progress_client_path = Path.Combine(path, project_file.data.items.progress_tracking.client);
				progress_client.path = progress_client_path;
				progress_client.load();

				var progress_supplier_path = Path.Combine(path, project_file.data.items.progress_tracking.supplier);
				progress_supplier.path = progress_supplier_path;
				progress_supplier.load();

				var progress_payments_path = Path.Combine(path, project_file.data.items.progress_tracking.payments);
				progress_payments.path = progress_payments_path;
				progress_payments.load();
			}

		}

		/***** PRIVATE *****/

	}
}
