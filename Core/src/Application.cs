﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using Core.utils;
using Core.Data;

namespace Core.src
{
	/* a Singleton class to manage runtime application */
	public class Application
	{
		/* singleton */
		private Application() { } 
		private static Application singleton;
		public static Application getSingleton() {
			if (singleton == null) singleton = new Application();
			return singleton;
		}

		/* attribute */
		private DataFile<ProjectData> project_file;


		/* initialization of all modules goes here */
		public void initialize()
		{
			if (!Directory.Exists(Paths.PROGRAMME_DATA)) Directory.CreateDirectory(Paths.PROGRAMME_DATA);
			if (!Directory.Exists(Paths.LOGS)) Directory.CreateDirectory(Paths.LOGS);
			/* TODO: initialize other modules */


			/* read the project file and apply the data */
			// project_file = new DataFile<ProjectData>(); // Directory.GetCurrentDirectory()
			// project_file.load(); // load project data to project_file.data
		}

		public void loadProjectFile() {
			foreach ( string file in Directory.GetFiles( Directory.GetCurrentDirectory() )) {
				Console.WriteLine(file);
			}
		}

	}
}
