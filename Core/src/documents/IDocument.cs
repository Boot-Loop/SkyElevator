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

	public interface IDocument
	{
		DocumentType getType();
		IDocumentData getData();
		bool isReadOnly();
		void open();	// open read the document and create/update the document data
		void save();	// save the document as per document data
		void close();	// unlock the document file and cleanup
	}

}
