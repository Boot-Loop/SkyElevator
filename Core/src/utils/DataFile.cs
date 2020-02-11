using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace Core.utils {
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
	 * 		DataFile<ProjectData> file = new DataFile(path_to_file); // path_to_file = @"C:\SE\proj_name\prjfile.se";
	 * 		file.save(data); // this will save the data to the file
	 * 		
	 * 		ProjectData loaded_obj = file.load(); // this will load from the path
	 *  }
	 * }
	 * 
	 * 
	 ***********************************/
	public class DataFile<DataClass>
	{
		private string file_path;
		private DataClass data;

		public DataFile(string file_path) {
			this.file_path = file_path;
		}
		public DataFile() { }

		public void setPath(string file_path) {
			this.file_path = file_path;
		}
		public string getPath() {
			return this.file_path;
		}

		public void setData(DataClass data) {
			if (data != null) this.data = data;
		}
		public DataClass getData() {
			return data;
		}

		public void save() {
			if (data == null) throw new Exception("data was null - use setData() or load()");
			save(data);
		}
		public void save(DataClass data) {
			if (this.file_path == null) throw new Exception("file_path was null");

			if (!this.data.Equals(data) && data != null) this.data = data;
			XmlSerializer serializer = new XmlSerializer(typeof(DataClass));
			using (TextWriter writer = new StreamWriter(this.file_path))
			{
				serializer.Serialize(writer, this.data);
			}
		}

		public DataClass load() {
			if (this.file_path == null) throw new Exception("file_path was null");

			XmlSerializer deserializer = new XmlSerializer(typeof(DataClass));
			using (TextReader reader = new StreamReader(this.file_path))
			{
				data = (DataClass)deserializer.Deserialize(reader);
				return data;
			}
		}

	}
}
