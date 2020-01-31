using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Core.src;

namespace Core.utils
{
    public class FileHandler
    {
		/* singleton */
		private FileHandler() { }
		private static FileHandler singleton;
		public static FileHandler getSingleton() {
			if (singleton == null) singleton = new FileHandler();
			return singleton;
		}

		public static void initialize() { /* TODO: impliment */ }

		// System.IO.File.ReadAllText(file_path);
		



		/* implement I/O interface for file access.
		 * sebe client request cache, downloaded respones cache
		 * also log files -> application, sebe client response logging
		 */
    }
	
	public class DirHandler
	{

		/* singleton */
		private DirHandler() { }
		private static DirHandler singleton;
		public static DirHandler getSingleton() {
			if (singleton == null) singleton = new DirHandler();
			return singleton;
		}
		public static void initialize() {
			if (!Directory.Exists(Ref.PROGRAMME_DATA_PATH)) Directory.CreateDirectory(Ref.PROGRAMME_DATA_PATH);
			if (!Directory.Exists(Ref.LOGS_PATH)) Directory.CreateDirectory(Ref.LOGS_PATH);
			/* TODO: impliment other dir handler initialize */
		}

		/* for menupulate folders:
		 * initialize project structures
		 * create and remove cache dirs
		 */

		/* initialize the pc for the first time and validate if the 
		 * pc initiaized before each application starts, and if not
		 * fix any missing dirs
		 */
		

		/* create project dir structure */
		//public static void createProject(String project_name, String project_path );

		/* Other implimentations... */
	}
}
