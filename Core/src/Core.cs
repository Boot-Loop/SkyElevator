using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Core.src
{
    
	public class Paths
	{
		/* if these dirs not exists -> create them on initialize */
		public static readonly string APP_DATA           = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
		public static readonly string PROGRAMME_DATA     = Path.Combine(APP_DATA, "SkyElevator/");
		public static readonly string LOGS               = Path.Combine(PROGRAMME_DATA, "logs/");

        public static readonly string EXECUTABLE         = new DirectoryInfo(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location)).FullName;
        public static readonly string DOCUMENT_TEMPLATE  = Path.Combine(EXECUTABLE, "templates");

        public class Template
        {
            public static readonly string INQUERY_SHEET             = Path.Combine(DOCUMENT_TEMPLATE, "inquery_sheet.docx");
            public static readonly string COMPLETION_REPORT         = Path.Combine(DOCUMENT_TEMPLATE, "completion_report.docx");
            public static readonly string WARRANTY_CERTIFICATION    = Path.Combine(DOCUMENT_TEMPLATE, "warranty_certification.docx");
        }

    }

    public class Reference
    {

    }

	/* create your custom exceptions here
	class CustomError : Exception
	{
		public CustomError(string name) : base("CustomError: " + name) { }
	}
	*/

	class NotLoggedInError : Exception
	{
		public NotLoggedInError() : base("NotLoggedInError: ") { }
		public NotLoggedInError(string name) : base("NotLoggedInError: " + name) { }
	}

	class ReadonlyError : Exception
	{
		public ReadonlyError() : base ("ReadonlyError: ") { }
		public ReadonlyError(string name) : base ("ReadonlyError: " + name) { }
	}

    class InvalidFileTypeError : Exception
    {
        public InvalidFileTypeError() : base("InvalidFileError") { }
        public InvalidFileTypeError(string name) : base ("InvalidFileError: " + name) { }
    }

    class InvalidPathError : Exception
    {
        public InvalidPathError() : base("InvalidFilePathError") { }
        public InvalidPathError(string name) : base("InvalidFilePathError: " + name) { }
    }

}
