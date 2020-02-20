using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Core.Data.Models
{
	public enum ModelType
	{
		PROJECT_MODEL,
		MODEL_CLIENT,
		MODEL_SUPPLIER,
		PROGRESS_CLIENT,
		PROGRESS_SUPPLIER,
		PROGRESS_PAYMENT,
	}


	[Serializable]
	public abstract class Model
	{
		public abstract bool matchPK(object pk);
		public abstract object getPK();
		public abstract ModelType getType();
		public abstract void save();

		public static Model newModel(ModelType model_type)
		{
			switch (model_type)
			{
				case ModelType.MODEL_CLIENT: 
					return new ClientModel();
				case ModelType.MODEL_SUPPLIER:
					throw new Exception("TODO: suppiler model didn't created yet");
				case ModelType.PROGRESS_CLIENT:
					throw new InvalidOperationException("cant create new progress client model");
				case ModelType.PROGRESS_SUPPLIER:
					throw new InvalidOperationException("cant create new progress supplier model");
				case ModelType.PROGRESS_PAYMENT:
					return new PaymentModel();
				case ModelType.PROJECT_MODEL:
					throw new InvalidOperationException("cant create new progress project model");
				default:
					throw new Exception("un handled model type " + model_type.ToString());
			}
		}


		public static Model getModel( object pk, ModelType model_type) {
			switch (model_type)
			{
				case ModelType.MODEL_CLIENT: {
						foreach (var client in Application.getSingleton().getClients()) {
							if (client.matchPK(pk)) { return client; }
						}
						throw new ModelNotExists("model for client not exists pk: " + pk.ToString());
					}
				case ModelType.MODEL_SUPPLIER: { // TODO: create an interface for supplier same as clients
						throw new Exception("TODO: supplier didn't created");
					}
				case ModelType.PROGRESS_CLIENT: {
						if (ProjectManager.singleton.hasProgressTracking())
							return ProjectManager.singleton.progress_client.data;
						throw new ArgumentException("this project has no tracking data");						
					}
				case ModelType.PROGRESS_SUPPLIER:
					{
						if (ProjectManager.singleton.hasProgressTracking()) 
							return ProjectManager.singleton.progress_supplier.data;
						throw new ArgumentException("this project has no tracking data");
					}
				case ModelType.PROGRESS_PAYMENT: {
						if (ProjectManager.singleton.hasProgressTracking()) {
							foreach (var payment in ProjectManager.singleton.progress_payments.data) {
								if (payment.matchPK(pk)) return payment;
							}
						}
						else throw new ArgumentException("this project has no tracking data");
						throw new ModelNotExists("model for progress payment not exists pk: " + pk.ToString());
					}
				default:
					throw new Exception("un handled model type " + model_type.ToString());
			}
		}


		public static void saveNewModel(Model model)
		{
			switch (model.getType())
			{
				case ModelType.MODEL_CLIENT:
					Application.getSingleton().clients_file.data.clients.Add((ClientModel)model);
					Application.getSingleton().clients_file.save();
					break;

				case ModelType.MODEL_SUPPLIER:
					throw new Exception("TODO: impliment");

				case ModelType.PROGRESS_PAYMENT:
					ProjectManager.singleton.progress_payments.data.Add((PaymentModel)model);
					ProjectManager.singleton.progress_payments.save();
					break;

				default:
					throw new InvalidOperationException("can't create new model for " + model.getType());

			}
		}

	}
}
