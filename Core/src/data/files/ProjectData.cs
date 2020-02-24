using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml.Serialization;

using Core.Data.Models;

namespace Core.Data.Files
{
	
	[Serializable]
	public class Items
	{
		[Serializable] public class InquirySheetItems { [XmlArrayItem("item")] public List<InqurySheetTag> clients; [XmlArrayItem("item")] public List<InqurySheetTag> suppliers; [XmlArrayItem("item")] public List<InqurySheetTag> drafts; }
		[Serializable] public class QuotationItems { [XmlArrayItem("item")] public List<QuotationTag> clients; [XmlArrayItem("item")] public List<QuotationTag> suppliers; [XmlArrayItem("item")] public List<QuotationTag> drafts; }
		[Serializable] public class SalesAgreementItems { [XmlArrayItem("item")] public List<SalesAgreementTag> clients; [XmlArrayItem("item")] public List<SalesAgreementTag> suppliers; [XmlArrayItem("item")] public List<SalesAgreementTag> drafts; }
		[Serializable] public class ProgressTrackingItems { public string client; public string supplier; public string payments;  } // each string is a relative path to the file
		public InquirySheetItems inquiry_sheets			= new InquirySheetItems();
		public QuotationItems quotations				= new QuotationItems();
		public SalesAgreementItems sales_agreements		= new SalesAgreementItems();
		public ProgressTrackingItems progress_tracking	= new ProgressTrackingItems();
		public string handover;
		public string maintenance;
	}

	[Serializable]
	public class ProjectData
	{

		public string application_version = Reference.VERSION;
		public ProjectModel project_model;

		public Items items = new Items();

		private ProjectData() { }
		public ProjectData( string project_name , ProjectModel project_model) { 
			this.project_model = project_model;
		}
	}
}
