using Core.Data.Files;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/** TODO:
 * create a model.validate method to validate
 * is_readonly modified, required is null, foreign key exists, ...
 */

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
		public abstract void saveUpdates();
		public abstract void saveNew();
		public abstract void validateRelation();

		public static ModelType toModelType(Type type)
		{
			if ( type == typeof(ProjectModel))				return ModelType.PROJECT_MODEL;
			if ( type == typeof(ClientModel))				return ModelType.MODEL_CLIENT;
			// TODO: supplier model if ( type == typeof() ) return 
			if ( type == typeof(ClientProgressModel))		return ModelType.PROGRESS_CLIENT;
			if ( type == typeof(SupplierProgressModel))		return ModelType.PROGRESS_SUPPLIER;
			if ( type == typeof(PaymentModel))				return ModelType.PROGRESS_PAYMENT;

			throw new Exception( "un handled model type or type is not model, Type : " + type.ToString() );
			
		}

		public static Model newModel(ModelType model_type)
		{
			switch (model_type)
			{
				case ModelType.PROJECT_MODEL:		return new ProjectModel();
				case ModelType.MODEL_CLIENT:		return new ClientModel();
				case ModelType.MODEL_SUPPLIER:		throw new Exception("TODO: suppiler model didn't created yet");
				case ModelType.PROGRESS_CLIENT:		return new ClientProgressModel();
				case ModelType.PROGRESS_SUPPLIER:	return new SupplierProgressModel();
				case ModelType.PROGRESS_PAYMENT:	return new PaymentModel();
				default:							throw new Exception("un handled model type " + model_type.ToString());
			}
		}


		public static Model getModel( object pk, ModelType model_type) {
			switch (model_type)
			{
				case ModelType.PROJECT_MODEL: {
						throw new InvalidOperationException("can't access project model with pk");
					}
				case ModelType.MODEL_CLIENT: {
						foreach (var client in Application.singleton.getClients()) {
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

	}
}
