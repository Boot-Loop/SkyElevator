﻿using System;
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
 * check pk already exists
 * save models data a dictionary { pk 1: model }
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
			
			Dictionary<string, object> json = new Dictionary<string, object>();
			switch (api_mode)
			{
				case ModelApiMode.MODE_CREATE: {
						foreach( PropertyInfo prop_info in model.GetType().GetRuntimeProperties()) {
							if ( prop_info.GetValue(model) is Field) { 
								Field field = prop_info.GetValue(model) as Field;
								if (field.isRequired() && field.isNull()) throw new RequiredFieldNullError( String.Format("model={0} field={1}", typeof(T).ToString(), field.getName()) );
								if (!field.isNull()) json.Add(field.getName(), field.getValue());
								field._setNotModified();
							}
						}
						generateUploadFiles(HttpRequestMethod.POST, model.getType(), model.getPK(), json);
						model.saveNew();
						break;
					}
				case ModelApiMode.MODE_READ:
					throw new NotImplementedException("read mode for api not implimented");
					// break;

				case ModelApiMode.MODE_UPDATE: {
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
						model.saveUpdates();
						break;
					}
				case ModelApiMode.MODE_DELETE: {
						/* TODO: delete the model from the disc : model.delete() or something */
						generateUploadFiles(HttpRequestMethod.DELETE, model.getType(), model.getPK(), null);
						break;
					}

			}
		}


		/* private methods */
		private void generateUploadFiles(HttpRequestMethod method, ModelType model_type, object pk = null, Dictionary<string, object> json = null) {
			DateTime date_time = DateTime.Now;
			UploadData upload_data = new UploadData(method, model_type, pk, json, date_time);
			var cache_file_name = date_time.Ticks.ToString() + ".dat";
			XmlFile<UploadData> upload_file = new XmlFile<UploadData>( file_path: Path.Combine(Core.Paths.UPLOAD_CACHE, cache_file_name), data : upload_data );
			upload_file.save();
			Application.singleton.programe_data_file.data.upload_cache_files.Add(cache_file_name);
			Application.singleton.programe_data_file.save();
		}

		private void setModel(object pk)
		{
			// if (_api_mode == ModelApiMode.MODE_CREATE) {
			// 		if (model_type == ModelType.PROGRESS_CLIENT || model_type == ModelType.PROGRESS_SUPPLIER) {
			// 			throw new ArgumentException( "can't create model : " + model_type.ToString() );
			// 		}
			// }
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
