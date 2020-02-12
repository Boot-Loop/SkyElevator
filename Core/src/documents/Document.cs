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
        COMPLETION_REPORT,
        WARRENTY_CERTIFICATION,
        ELEVATOR_INSPECTION_SHEET
	}
	public interface IDocumentData
	{
		void setToDefault();
		DocumentType getType();
	}

    public abstract class Document
    {
        protected bool is_readonly = false;
        protected string path = null;

        public abstract DocumentType getType();
        public abstract IDocumentData getData();
        public abstract void saveAsDraft(); // save the document as per document data
        public abstract void close();   // unlock the document file and cleanup
        public abstract void loadDocument(bool is_readonly = false);
        public abstract void generateDocument(string path);

        public Document() { }
        public Document(string path) {
            if (!Validator.validateFilePath(path, is_new: true)) throw new InvalidPathError();
            this.path = path;
        }

        public static void checkDocument(string path, DocumentType type)
        {
            if (!Validator.validateFilePath(path, is_new: false)) throw new InvalidPathError();
            // TODO: implement here

        }

        public string getPath() => path;
        public void setPath(string path) {
            if (!Validator.validateFilePath(path, is_new: true)) throw new InvalidPathError();
            this.path = path;
        }
        public bool isReadonly() => is_readonly;
        public void setReadonly(bool _readonly) => is_readonly = _readonly;
    }
}
