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
 * save models data a dictionary { pk 1: model }
 * may be save attachment file to cache
 */

namespace Core.Data
{
	public enum ModelApiMode
	{
		MODE_CREATE,
		MODE_UPDATE,
		MODE_READ,
		MODE_DELETE,
	}

	public class ModelAPI<T> where T : Model
	{
		private ModelType model_type;
		public  T model { get; set; } = null;

		private ModelApiMode _api_mode = ModelApiMode.MODE_READ;
		public ModelApiMode api_mode { get { return _api_mode; } } 

		public ModelAPI(object pk, ModelApiMode api_mode) {
			_api_mode	= api_mode;
			this.model_type = Model.toModelType(typeof(T));
			setModel(pk);
		}

		public void update() {
			
			Dictionary<string, string> request_data = new Dictionary<string, string>();
			Dictionary<string, string> attachments	= new Dictionary<string, string>();
			switch (api_mode)
			{
				case ModelApiMode.MODE_CREATE: {
						foreach( PropertyInfo prop_info in model.GetType().GetRuntimeProperties()) {
							if ( prop_info.GetValue(model) is Field) { 
								Field field = prop_info.GetValue(model) as Field;
								if (field.isRequired() && field.isNull()) throw new RequiredFieldNullError( String.Format("model={0} field={1}", typeof(T).ToString(), field.getName()) );
								if (!field.isNull()) {
									object value = field.getValue();
									if (!(value is String)) value = JsonSerializer.Serialize(value);
									if (field is FileField) {
										if (!Application.singleton.is_proj_loaded) throw new Exception("project must be loaded to use ModelAPI for attachments");
										var f_field = field as FileField;
										string file_name = model.getPK().ToString() + getFileExtension(Path.GetFullPath(f_field.value));
										File.Copy(Path.GetFullPath(f_field.value), Path.Combine( Paths.ATTACHMENT_CACHE,  file_name ));
										attachments.Add(f_field.getName(), file_name);
									}
									else request_data.Add(field.getName(), value.ToString());
								}
								field._setNotModified();
							}
						}
						model.validateRelation();
						model.saveNew();
						generateUploadFiles(HttpRequestMethod.POST, model.getType(), model.getPK(), request_data, attachments);
						break;
					}
				case ModelApiMode.MODE_READ:
					throw new InvalidOperationException("can't update api when in read mode");
					// break; // TODO: maybe check if any field modfied

				case ModelApiMode.MODE_UPDATE: {
						foreach (PropertyInfo prop_info in model.GetType().GetRuntimeProperties()) {
							if (prop_info.GetValue(model) is Field) {
								Field field = prop_info.GetValue(model) as Field;
								if (field.isModified()) {
									if (field is FileField) {
										if (!Application.singleton.is_proj_loaded) throw new Exception("project must be loaded to use ModelAPI for attachments");
										var f_field = field as FileField;
										string file_name = model.getPK().ToString() + getFileExtension(Path.GetFullPath(f_field.value));
										File.Copy(Path.GetFullPath(f_field.value), Path.Combine(Paths.ATTACHMENT_CACHE, file_name));
										attachments.Add(f_field.getName(), file_name);
									}
									else request_data.Add(field.getName(), JsonSerializer.Serialize(field.getValue()));
									field._setNotModified();
								}
							}
						}
						model.validateRelation();
						model.saveUpdates();
						generateUploadFiles(HttpRequestMethod.PATCH, model.getType(), model.getPK(), request_data, attachments);
						break;
					}
				case ModelApiMode.MODE_DELETE: {
						model.delete();
						generateUploadFiles(HttpRequestMethod.DELETE, model.getType(), model.getPK(), null, null);
						break;
					}
			}
		}


		/* private methods */
		private string getFileExtension(string path) {
			if (!path.Contains(".")) throw new Exception("unable to get file extension -> path does not contain a dot");
			var list = path.Split('.');
			return "."+list[list.Length - 1];
		}

		private void generateUploadFiles(HttpRequestMethod method, ModelType model_type, object pk = null, Dictionary<string, string> request_data = null, Dictionary<string, string> attachments = null) {
			DateTime date_time = DateTime.Now;
			UploadData upload_data = new UploadData(method, model_type, pk, request_data, attachments, date_time);
			var cache_file_name = date_time.Ticks.ToString() + ".dat";
			XmlFile<UploadData> upload_file = new XmlFile<UploadData>( file_path: Path.Combine(Core.Paths.UPLOAD_CACHE, cache_file_name), data : upload_data );
			upload_file.save();
			Application.singleton.programe_data_file.data.upload_cache_files.Add(cache_file_name);
			Application.singleton.programe_data_file.save();
		}

		private void setModel(object pk)
		{
			if (_api_mode != ModelApiMode.MODE_CREATE) { // need pk
				if (pk is null) throw new ArgumentNullException("required argument pk");
			}

			if (api_mode == ModelApiMode.MODE_CREATE) {
				if (!(pk is null)) Logger.logger.logWarning("for model creation mode pk is not-required");
				model = Model.newModel(model_type) as T;
			}
			else model = Model.getModel(pk, model_type) as T;
			
		}
	}
}
