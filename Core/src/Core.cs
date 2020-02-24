using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using Core.Utils;

namespace Core
{
    
	public class Paths
	{
		/* if these dirs not exists -> create them on initialize */
		public static readonly string APP_DATA              = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
		public static readonly string PROGRAMME_DATA        = Path.Combine(APP_DATA, "SkyElevator/");
        public static readonly string DEFAULT_PROJ_DIR      = Path.Combine( Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "SkyElevator/" );
		public static readonly string LOGS                  = Path.Combine(PROGRAMME_DATA,  "Logs/"             );
		public static readonly string SEBE_CLIENT           = Path.Combine(PROGRAMME_DATA,  "Sebe Client/"      );
        public static readonly string UPLOAD_CACHE          = Path.Combine(SEBE_CLIENT,     "Upload Cache/"     );
        public static readonly string ATTACHMENT_CACHE      = Path.Combine(SEBE_CLIENT,     "Attachment Cache"  );

        public static readonly string EXECUTABLE            = new DirectoryInfo(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location)).FullName;
        public static readonly string DOCUMENT_TEMPLATE     = Path.Combine(EXECUTABLE,      "templates"         );
        public static readonly string PROGRAME_DATA_FILE    = Path.Combine(PROGRAMME_DATA,  "ProgrammeData.xml" );
        public static readonly string CLIENTS_DATA_FILE     = Path.Combine(PROGRAMME_DATA,  "clients.dat"       );
        public static readonly string SUPPLIERS_DATA_FILE   = Path.Combine(PROGRAMME_DATA,  "suppliers.dat"     );

        public class Template
        {
            public static readonly string INQUERY_SHEET             = Path.Combine(DOCUMENT_TEMPLATE, "inquery_sheet.docx"              );
            public static readonly string COMPLETION_REPORT         = Path.Combine(DOCUMENT_TEMPLATE, "completion_report.docx"          );
            public static readonly string WARRANTY_CERTIFICATION    = Path.Combine(DOCUMENT_TEMPLATE, "warranty_certification.docx"     );
            public static readonly string ELEVATOR_INSPECTION_SHEET = Path.Combine(DOCUMENT_TEMPLATE, "elevator_inspection_sheet.docx"  );
        }

    }

    public class Reference
    {
        public static readonly string VERSION                   = "1.0.0";
        public static readonly string PROJECT_FILE_EXTENSION    = ".skyproj";
        public static Logger uilogger { get; }                  = new Logger("ui.logs");
    }

    public enum HttpRequestMethod
    {
        GET, POST, DELETE, PUT, PATCH
    }

    /* custom exceptions */

    /* sebe client */
	public class NotLoggedInError : Exception
	{
		public NotLoggedInError() : base("NotLoggedInError") { }
		public NotLoggedInError(string name) : base("NotLoggedInError: " + name) { }
	}

	public class HttpNotFoundError : Exception
    {
        public HttpNotFoundError() : base("HttpNotFoundError") { }
        public HttpNotFoundError(string name) : base("HttpNotFoundError: " + name) { }
    }

    public class HttpBadRequestError : Exception
    {
        public HttpBadRequestError() : base("HttpBadRequestError") { }
        public HttpBadRequestError(string name) : base("HttpBadRequestError: " + name) { }
    }


    /* document errors */
	public class ReadonlyError : Exception
	{
		public ReadonlyError() : base ("ReadonlyError") { }
		public ReadonlyError(string name) : base ("ReadonlyError: " + name) { }
	}

	public class InvalidFileTypeError : Exception
    {
        public InvalidFileTypeError() : base("InvalidFileError") { }
        public InvalidFileTypeError(string name) : base ("InvalidFileError: " + name) { }
    }

	public class InvalidPathError : Exception
    {
        public InvalidPathError() : base("InvalidPathError") { }
        public InvalidPathError(string name) : base("InvalidPathError: " + name) { }
    }

    /* model errors */
    public class ModelNotExists : Exception
    {
        public ModelNotExists() : base("ModelNotExists") { }
        public ModelNotExists(string name) : base("ModelNotExists: " + name) { }
    }

    public class RequiredFieldNullError : Exception
    {
        public RequiredFieldNullError() : base("RequiredFieldNullError") { }
        public RequiredFieldNullError(string name) : base("RequiredFieldNullError: " + name) { }
    }



    /* other errors */
    public class AlreadyExistsError : Exception
    {
        public AlreadyExistsError() : base("AlreadyExistsError") { }
        public AlreadyExistsError(string name) : base("AlreadyExistsError: " + name) { }
    }

    public class ValidationError : Exception
    {
        public ValidationError() : base("ValidationError") { }
        public ValidationError(string name) : base("ValidationError: " + name) { }
    }

    


}
