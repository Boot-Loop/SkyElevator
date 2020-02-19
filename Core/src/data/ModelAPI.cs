using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Core.Data.Models;

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

		public  Model repr_model { get; set; } = null;
		private Model real_model { get; set; } = null;

		private APIMode _api_mode = APIMode.MODE_READ;
		private ModelType model_type;
		public APIMode api_mode { get { return _api_mode; } } 

		public ModelAPI(Model model, object pk = null, ModelType model_type = ModelType.MODEL_CLIENT, APIMode api_mode = APIMode.MODE_READ) {
			repr_model = model;
			_api_mode	= api_mode;
			if (model != null) this.model_type = model.getType();
			else this.model_type = model_type;
			setRealModel(pk);
		}




		/* private methods */
		private void setRealModel(object pk)
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

			if (api_mode == APIMode.MODE_CREATE) return;

			switch (model_type)
			{
				case ModelType.MODEL_CLIENT: {
						foreach (var client in Application.getSingleton().getClients()) {
							if (client.matchPK(pk)) { this.real_model = client; break; }
						}
						throw new ModelNotExists("model for client not exists pk: " + pk.ToString());
					}
				case ModelType.MODEL_SUPPLIER: { // TODO: create an interface for supplier same as clients
						break;
					}
				case ModelType.PROGRESS_CLIENT: {
						if (ProjectManager.singleton.hasProgressTracking()) {
							real_model = ProjectManager.singleton.progress_client.data;
						} else {
							throw new ArgumentException("this project has no tracking data");
						}
						break;
					}
				case ModelType.PROGRESS_SUPPLIER: {
						if (ProjectManager.singleton.hasProgressTracking()) {
							real_model = ProjectManager.singleton.progress_supplier.data;
						} else {
							throw new ArgumentException("this project has no tracking data");
						}
						break;
					}
				case ModelType.PROGRESS_PAYMENT: {
						if (ProjectManager.singleton.hasProgressTracking()) {
							foreach (var payment in ProjectManager.singleton.progress_payments.data) {
								if (payment.matchPK(pk)) real_model = payment;
								break;
							}
						} else {
							throw new ArgumentException("this project has no tracking data");
						}
						throw new ModelNotExists("model for progress payment not exists pk: " + pk.ToString());
					}
			}
		}
	}
}
