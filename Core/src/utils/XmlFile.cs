using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace Core.Utils {
	/* ********** USAGE ****************
	 * 
	 * constructor:
	 *		DataFile<DataClass>(string file_path);
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
	 * 		XmlFile<ProjectData> file = new DataFile(path_to_file); // path_to_file = @"C:\SE\proj_name\prjfile.se";
	 * 		file.save(data); // this will save the data to the file
	 * 		
	 * 		ProjectData loaded_obj = file.load(); // this will load from the path
	 *  }
	 * }
	 * 
	 * 
	 ***********************************/
	public class XmlFile<DataClass>
	{
		static Logger logger = new Logger();

		public string path { get; set; } = null;
		public DataClass data	{ get; set; } = default(DataClass);

		public XmlFile() { }
		public XmlFile(string file_path = null, DataClass data = default(DataClass)) { this.path = file_path; this.data = data; }
		
		public void save() {
			if (this.path == null) logger.logError("save xml file path was null");
			if ( this.data.Equals(default(DataClass)) ) logger.logError("data was default(DataClass) : null data");
			XmlSerializer serializer = new XmlSerializer(typeof(DataClass));
			using (TextWriter writer = new StreamWriter(this.path)) {
				serializer.Serialize(writer, this.data);
			}
		}

		/// <summary>
		/// throws :
		///		InvalidOperationException - if the xml file is currupted
		///		FileNotFoundException     - if the file not found
		///		ArgumentNullException     - if the file_path is null
		/// </summary>
		/// <returns></returns>
		public DataClass load() {
			if (this.path == null) logger.logError("save xml file path was null");
			XmlSerializer deserializer = new XmlSerializer(typeof(DataClass));
			using (TextReader reader = new StreamReader(this.path)) {
				data = (DataClass)deserializer.Deserialize(reader);
				return data;
			}
		}

	}
}
