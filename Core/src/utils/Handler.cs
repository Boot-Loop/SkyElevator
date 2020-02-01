using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Core.src;

namespace Core.utils
{
    public sealed class FileHandler
    {
		/* singleton */
		private FileHandler() { }
		private static readonly FileHandler singleton = new FileHandler();
		public static FileHandler Instance {
			get
			{
				return singleton;
			}
		}

		public static void initialize() { /* TODO: impliment */ }

		// System.IO.File.ReadAllText(file_path);
		

		/* implement I/O interface for file access.
		 * sebe client request cache, downloaded respones cache
		 * also log files -> application, sebe client response logging
		 */

		/* Copy a File */
		public void CopyFile(string sourceFileName, string destinationFileName, bool overwrite) {

			File.Copy(sourceFileName, destinationFileName, overwrite);
			/* System.IO.IOException when rewrite parameter is set to be false is not handled here */
		}
    }
	
	public sealed class DirHandler
	{

		/* singleton */
		private DirHandler() { }
		private static readonly DirHandler singleton = new DirHandler();
		public static DirHandler Instance {
			get {
				return singleton;
			}
		}

		/* Dir Handler Initialization */
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


		/* Create Project Directories */

		public void CreateProject(string projectName, string filePath) {

			string projectDirectory = Path.Combine(filePath, projectName);
			Directory.CreateDirectory(projectDirectory);
	
			Dictionary<string, bool> directories = new Dictionary<string, bool>() {
				{ "Inquiry Sheet", false },
				{ "Quotation", true },
				{ "Sales Agreement", true },
				{ "Project Tracking", false },
				{ "Handover", false },
				{ "Maintenance", false }
			};
			List<string> subDirectories = new List<string>() { 
				"Client",
				"Supplier"
			};

			foreach (KeyValuePair<string, bool> directory in directories) {
				Directory.CreateDirectory(Path.Combine(projectDirectory, directory.Key));
				if (directory.Value) {
					foreach (string subDirectory in subDirectories) {
						Directory.CreateDirectory(Path.Combine(projectDirectory, directory.Key, subDirectory));
					}
				}
			}
		}

		/* Other implimentations... */
	}
}
