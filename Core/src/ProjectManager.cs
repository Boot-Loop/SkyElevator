using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.src
{


	public class ProjectManager
	{
		/*
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
		*/
	}
}
