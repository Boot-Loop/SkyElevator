using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using Core.utils;

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
		//void copyFrom(IDocumentData data);
		DocumentType getType();
	}

	public abstract class Document
	{
		protected bool is_readonly = false;
		protected string path = null;

		public abstract DocumentType getType();
		public abstract IDocumentData getData();
		public abstract void saveAsDraft();	// save the document as per document data
		public abstract void close();   // unlock the document file and cleanup
        public abstract void loadDocument();
        public abstract void checkDocumentType();

        public Document() { }
        public Document(string path) {
            var dir_name = new DirectoryInfo(System.IO.Path.GetDirectoryName(path)).FullName;
            if (!Validator.validatePath( dir_name ) ) throw new InvalidFilePathError(); this.path = path;
        }

		public string getPath() => path;
        public void setPath(string path) {
            var dir_name = new DirectoryInfo(System.IO.Path.GetDirectoryName(path)).FullName;
            if (!Validator.validatePath(dir_name)) throw new InvalidFilePathError();
            this.path = path;
        }
		public bool isReadonly() => is_readonly;
		public void setReadonly(bool _readonly) => is_readonly = _readonly;
	}

}
