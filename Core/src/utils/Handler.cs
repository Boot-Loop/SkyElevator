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
		private static readonly FileHandler _singleton = new FileHandler();
		public static FileHandler singleton {
			get { return _singleton; }
		}

		public static void initialize() { /* TODO: impliment */ }
		

		/* implement I/O interface for file access.
		 * sebe client request cache, downloaded respones cache
		 */

    }
	
	public sealed class DirHandler
	{

		/* singleton */
		private DirHandler() { }
		private static readonly DirHandler _singleton = new DirHandler();
		public static DirHandler singleton {
			get { return _singleton; }
		}

		/* Dir Handler Initialization */
		public static void initialize() {
			if (!Directory.Exists(Paths.PROGRAMME_DATA)) Directory.CreateDirectory(Paths.PROGRAMME_DATA);
			if (!Directory.Exists(Paths.LOGS)) Directory.CreateDirectory(Paths.LOGS);
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
