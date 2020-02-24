using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;


namespace Core.Data.Models
{
	[Serializable]
	public class InqurySheetTag
	{
		[XmlAttribute] public long	id;
		[XmlAttribute] public string doc;
		public string comments;

		public InquirySheetModel asModel(bool is_client) {
			var data = ProjectManager.singleton.project_file.data;
			if (data is null) throw new Exception("InquriySheetTag.asModel() - did you load any project?");
			InquirySheetModel ret = new InquirySheetModel();
			ret.id.value			= id;
			ret.project_id.value	= data.project_model.id.value;
			ret.is_client.value		= is_client;
			ret.document.value		= doc;
			ret.comments.value		= comments;
			return ret;
		}
	}

	[Serializable]
	public class InquirySheetModel : Model
	{
		public IntergerField id				{ get; set; } = new IntergerField(	name: "id",			is_required: true	);
		public IntergerField project_id		{ get; set; } = new IntergerField(	name: "project",	is_required: true	);
		public FileField document			{ get; set; } = new FileField(		name: "document",	is_required: true	);
		public BoolField is_client			{ get; set; } = new BoolField(		name: "is_client",	is_required: true	);
		public TextField comments			{ get; set; } = new FileField(		name: "comments"						);

		// public string draft_path { get; set; }

		public InqurySheetTag asTag() {
			return new InqurySheetTag() {
				id			= id.value,
				doc			= document.value,
				comments	= comments.value
			};
		}

		/* overrides */
		override public ModelType getType() => ModelType.INQUIRY_SHEET;
		public override bool matchPK(object pk) => id.value == Convert.ToInt64(pk);
		public override object getPK() => id.value;
		public override void saveUpdates() => throw new InvalidOperationException("inquiry sheets can't be edited");

		public override void validateRelation() {
			var data = ProjectManager.singleton.project_file.data;
			if (data is null) throw new Exception("InquriySheetTag.asModel() - did you load any project?");
			if (data.project_model.id.value != project_id.value) throw new ValidationError("project id didn't match with inquiry sheet'k fk");
		}
		
		public override void saveNew()
		{
			if (!ProjectManager.singleton.hasInquirySheet()) throw new InvalidOperationException("project has no inquiry sheet");
			// if (draft_path is null) throw new NullReferenceException("drafts path was null for inquiry sheet");
			// TODO: save inquiry sheet from the drafts path to this.document.value
			var proj_file = ProjectManager.singleton.project_file;
			proj_file.data.items.inquiry_sheets.clients.Add(asTag());
			proj_file.save();
		}

		public override void delete()
		{
			if (!ProjectManager.singleton.hasInquirySheet()) throw new InvalidOperationException("project has no inquiry sheet");
			List<InqurySheetTag> list;
			if (is_client.value) list = ProjectManager.singleton.project_file.data.items.inquiry_sheets.clients;
			else list = ProjectManager.singleton.project_file.data.items.inquiry_sheets.suppliers;			

			InqurySheetTag to_remove = null;
			foreach ( var tag in list) {
				if (tag.id == this.id.value) to_remove = tag;
			}
			if (to_remove is null) throw new Exception("inquiry sheet tag not exists on project file");
			list.Remove(to_remove); ProjectManager.singleton.project_file.save();
			File.Delete(Path.Combine(ProjectManager.singleton.project_dir, document.value));
		}
	}
}
