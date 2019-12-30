using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace SkyElevator.src.utils
{
	/************ USAGE ****************
	 * 
	 * constructor:
	 *		ProjectFile<DataClass>(string file_path);
	 * 
	 * methods:
	 *		void setPath(string path);
	 *		void getPath(string path);
	 *		void save();
	 *		DataClass load();
	 * 
	 * *********************************
	 * 
	 * // define a class with all necessary data
	 * 
	 * class ProjectData
	 * {
	 *		string name;
	 *		int size;
	 *		List<string> tab_names;
	 *		UiDetail detail;
	 *		// other attributes
	 * }
	 * 
	 * class MainClass {
	 * 
	 *  void Main() {
	 * 		ProjectData data = new ProjectData( args );
	 * 		data.setYourValues();
	 * 		
	 * 		ProjectFile<ProjectData> file = new ProjectFile(path_to_file); // path_to_file = @"C:\SE\proj_name\prjfile.se";
	 * 		file.save(data); // this will save the data to the file
	 * 		
	 * 		ProjectData loaded_obj = file.load(); // this will load from the path
	 *  }
	 * }
	 * 
	 * 
	 ***********************************/
	public class ProjectFile<DataClass>
	{
		private string file_path;
		ProjectFile(string file_path){
			this.file_path = file_path;
		}

		public void setPath(string file_path)
		{
			this.file_path = file_path;
		}

		public string getPath()
		{
			return this.file_path;
		}

		public void save(DataClass data)
		{
			XmlSerializer serializer = new XmlSerializer(typeof(DataClass));
			using (TextWriter writer = new StreamWriter(this.file_path))
			{
				serializer.Serialize(writer, data);
			}
		}

		public DataClass load()
		{
			XmlSerializer deserializer = new XmlSerializer(typeof(DataClass));
			using (TextReader reader = new StreamReader(this.file_path))
			{
				DataClass data = (DataClass)deserializer.Deserialize(reader);
				return data;
			}
		}

	}
}
