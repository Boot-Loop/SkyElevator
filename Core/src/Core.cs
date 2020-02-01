using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Core.src
{
	public class Ref
	{
		/* if these dirs not exists -> create them on initialize */
		public static readonly string APP_DATA_PATH = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
		public static readonly string PROGRAMME_DATA_PATH = Path.Combine(APP_DATA_PATH, "SkyElevator/");
		public static readonly string LOGS_PATH = Path.Combine(PROGRAMME_DATA_PATH, "logs/");
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

}
