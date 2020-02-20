using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Reflection;
using System.IO;

using Core.Data.Models;
using Core.Data.Files;
using Core.Utils;

/** TODO:
 * modified field -> check value has changed
 * required fields null throw for create mode
 * if field null dont add to dictionary
 * check pk already exists
 */

namespace Core.Data
{
	public class ModelAPI
	{
		public enum APIMode
		{
			MODE_CREATE,
			MODE_UPDATE,
			MODE_READ,
			MODE_DELETE,
		}

		private ModelType model_type;
		public  Model model { get; set; } = null;

		private APIMode _api_mode = APIMode.MODE_READ;
		public APIMode api_mode { get { return _api_mode; } } 

		public ModelAPI(object pk, ModelType model_type, APIMode api_mode) {
			_api_mode	= api_mode;
			this.model_type = model_type;
			setModel(pk);
		}

		public void update() {
			
			Dictionary<string, object> json = new Dictionary<string, object>();
			switch (api_mode)
			{
				case APIMode.MODE_CREATE: {
						foreach( PropertyInfo prop_info in model.GetType().GetRuntimeProperties()) {
							if ( prop_info.GetValue(model) is Field) { 
								Field field = prop_info.GetValue(model) as Field;
								if (field.isRequired() && field.isNull()) throw new RequiredFieldNullError("field : " + field.ToString() );
								if (!field.isNull())
									json.Add(field.getName(), field.getValue());
							}
						}
						generateUploadFiles(HttpRequestMethod.POST, model.getType(), model.getPK(), json);
						Model.saveNewModel(model);
						break;
					}
				case APIMode.MODE_READ:
					throw new NotImplementedException("read mode for api not implimented");
					// break;

				case APIMode.MODE_UPDATE: {
						foreach (PropertyInfo prop_info in model.GetType().GetRuntimeProperties()) {
							if (prop_info.GetValue(model) is Field) {
								Field field = prop_info.GetValue(model) as Field;
								if (field.isModified()) {
									json.Add(field.getName(), field.getValue());
									field._setNotModified();
								}
							}
						}
						generateUploadFiles(HttpRequestMethod.PATCH, model.getType(), model.getPK(), json);
						model.save();
						break;
					}
				case APIMode.MODE_DELETE: {
						generateUploadFiles(HttpRequestMethod.DELETE, model.getType(), model.getPK(), json);
						break;
					}

			}
		}


		/* private methods */
		private void generateUploadFiles(HttpRequestMethod method, ModelType model_type, object pk = null, Dictionary<string, object> json = null) {
			DateTime date_time = DateTime.Now;
			UploadData upload_data = new UploadData(method, model_type, pk, json, date_time);
			var cache_dir_path = ProjectManager.singleton.project_file.data.dirs.getDir(ProjectManager.Dirs.DOT_SKY_DIR).getDir(ProjectManager.Dirs.SEBE_CACHE).path;
			var cache_file_name = date_time.Ticks.ToString() + ".dat";
			XmlFile<UploadData> upload_file = new XmlFile<UploadData>( file_path: Path.Combine(cache_dir_path, cache_file_name), data : upload_data );
			upload_file.save();
			ProjectManager.singleton.project_file.data.dirs.getDir(ProjectManager.Dirs.DOT_SKY_DIR).getDir(ProjectManager.Dirs.SEBE_CACHE).addFile(cache_file_name);
			ProjectManager.singleton.project_file.save();
		}

		private void setModel(object pk)
		{
			// TODO: delete, modification need access

			if (model_type == ModelType.PROJECT_MODEL) throw new ArgumentException("can't create model for project");
			if (_api_mode == APIMode.MODE_CREATE) {
				if (model_type == ModelType.PROGRESS_CLIENT || model_type == ModelType.PROGRESS_PAYMENT || model_type == ModelType.PROGRESS_SUPPLIER) {
					throw new ArgumentException( "can't create model : " + model_type.ToString() );
				}
			}
			if (_api_mode != APIMode.MODE_CREATE) { // need pk
				if (model_type == ModelType.MODEL_CLIENT || model_type == ModelType.MODEL_SUPPLIER
					|| model_type == ModelType.PROGRESS_PAYMENT
				) {
					if (pk is null) throw new ArgumentNullException("required argument pk");
				}
			}

			if (api_mode == APIMode.MODE_CREATE) {
				if (!(pk is null)) Logger.logger.logWarning("for model creation mode pk is not-required");
				model = Model.newModel(model_type);
			}
			else model = Model.getModel(pk, model_type);
			
		}
	}
}
