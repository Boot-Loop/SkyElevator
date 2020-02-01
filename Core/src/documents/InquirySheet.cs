using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.src.documents
{
	public class InquirySheetData : IDocumentData
	{
		FloatField		speed			= new FloatField("Speed");
		IntergerField	capacity		= new IntergerField("Capacity");
		DimensionField	travel_height	= new DimensionField("Travel Height");
		
		/* constructor */
		public InquirySheetData() { }

		public void copyFrom(IDocumentData data) { // this method might be unnecessary!!
			if (data.getType() != DocumentType.INQUERY_SHEET) throw new ArgumentException();
			InquirySheetData idata = (InquirySheetData)data;
			this.speed.value	= idata.speed.value;
			this.capacity.value = idata.capacity.value;
			this.travel_height.value = idata.capacity.value;
			/* TODO: copy every thing from idata to this */

		}

		public void setToDefault() {
			/* TODO: reset all fields to the value of inquiry sheet's default value */
			this.speed.value			= 12;
			this.capacity.value			= 4;
			this.travel_height.value	= 3000;
		}

		public DocumentType getType() => DocumentType.INQUERY_SHEET;

	} // InquirySheetData


	/******* IMPLIMENTATION OF INQUIRY SHEET ********/
	public class InquirySheet : Document
	{
		/* fields */
		private InquirySheetData data = new InquirySheetData();

		/* methods */
		public InquirySheet(string path) : base(path) {
			// TODO: open the file at the path and update data;
		}

		override public DocumentType getType()  => DocumentType.INQUERY_SHEET;
		override public IDocumentData getData() => data;

		override public void close() {
			throw new NotImplementedException();
		}

		override public void save() {
			throw new NotImplementedException();
		}
	}
}
