using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.src.documents
{
	public enum DocumentType
	{
		INQUERY_SHEET,
		QUOTATION,
		SALES_AGREEMENT,
		HANDOVER,
	}
	public interface IDocumentData
	{
		void setToDefault();
		void copyFrom(IDocumentData data);
		DocumentType getType();
	}

	public abstract class Document
	{
		protected bool is_readonly = false;
		protected string path = "";

		public abstract DocumentType getType();
		public abstract IDocumentData getData();
		public abstract void save();	// save the document as per document data
		public abstract void close();	// unlock the document file and cleanup

		public Document(string path) {
			this.path = path;
			/* open the file at the path when inheriting */
		}
		public string getPath() => path;
		public bool isReadonly() => is_readonly;
		public void setReadonly(bool _readonly) => is_readonly = _readonly;
	}

}
