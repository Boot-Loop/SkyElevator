﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace Core.Utils
{
	public class BinFile<DataClass>
	{
		static Logger logger = new Logger();

		public string file_path { get; set; } = null;
		public DataClass data	{ get; set; } = default(DataClass);

		public BinFile() { }
		public BinFile(string file_path = null, DataClass data = default(DataClass)) { this.file_path = file_path; this.data = data; }

		public void save() {
			if (file_path == null) logger.logError("save xml file path was null");
			if (data.Equals(default(DataClass))) logger.logError("data was default(DataClass) : null data");
			using (FileStream stream = File.Open(file_path, FileMode.Create)) {
				BinaryFormatter bformater = new BinaryFormatter();
				bformater.Serialize(stream, data);
			}
		}

		public DataClass load() {
			using (FileStream stream = File.Open(file_path, FileMode.Open)) {
				BinaryFormatter bformater = new BinaryFormatter();
				data = (DataClass)bformater.Deserialize(stream);
				return data;
			}
		}
	}
}
