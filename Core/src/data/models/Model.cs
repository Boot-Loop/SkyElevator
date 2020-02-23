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
		public class AppNames
		{
			private AppNames() { }
			public static readonly string ACCOUNTS	= "accounts";
			public static readonly string DOCUMENTS = "documents";
			public static readonly string PROJECTS	= "projects";
		}

		public abstract bool matchPK(object pk);
		public abstract object getPK();
		public abstract ModelType getType();
		public abstract void saveUpdates();
		public abstract void saveNew();
		public abstract void delete();
		public abstract void validateRelation();

		public static ModelType toModelType(Type type)
		{
			if ( type == typeof(ProjectModel))				return ModelType.PROJECT_MODEL;
			if ( type == typeof(ClientModel))				return ModelType.MODEL_CLIENT;
			if ( type == typeof(SupplierModel))				return ModelType.MODEL_SUPPLIER;
			if ( type == typeof(ClientProgressModel))		return ModelType.PROGRESS_CLIENT;
			if ( type == typeof(SupplierProgressModel))		return ModelType.PROGRESS_SUPPLIER;
			if ( type == typeof(PaymentModel))				return ModelType.PROGRESS_PAYMENT;

			throw new Exception( "un handled model type or type is not model, Type : " + type.ToString() );
			
		}

		public static Model newModel(ModelType model_type)
		{
			switch (model_type)
			{
				case ModelType.PROJECT_MODEL:		return	new ProjectModel();
				case ModelType.MODEL_CLIENT:		return	new ClientModel();
				case ModelType.MODEL_SUPPLIER:		return	new SupplierModel();
				case ModelType.PROGRESS_CLIENT:		return	new ClientProgressModel();
				case ModelType.PROGRESS_SUPPLIER:	return	new SupplierProgressModel();
				case ModelType.PROGRESS_PAYMENT:	return	new PaymentModel();
				default:							throw	new Exception("un handled model type " + model_type.ToString());
			}
		}

		public static string getAppName(ModelType model_type)
		{
			switch (model_type)
			{
				case ModelType.PROJECT_MODEL:		return Model.AppNames.PROJECTS;
				case ModelType.MODEL_CLIENT:		return Model.AppNames.ACCOUNTS;
				case ModelType.MODEL_SUPPLIER:		return Model.AppNames.ACCOUNTS;
				case ModelType.PROGRESS_CLIENT:		return Model.AppNames.DOCUMENTS;
				case ModelType.PROGRESS_SUPPLIER:	return Model.AppNames.DOCUMENTS;
				case ModelType.PROGRESS_PAYMENT:	return Model.AppNames.PROJECTS;
				default: throw new Exception("un handled model type : " + model_type.ToString());
			}
		}

		public static string getVerboseSinglar(ModelType model_type)
		{
			switch (model_type)
			{
				case ModelType.PROJECT_MODEL:		return "project";
				case ModelType.MODEL_CLIENT:		return "client";
				case ModelType.MODEL_SUPPLIER:		return "supplier";
				case ModelType.PROGRESS_CLIENT:		return "progress-client-file";
				case ModelType.PROGRESS_SUPPLIER:	return "progress-supplier-file";
				case ModelType.PROGRESS_PAYMENT:	return "";
				default: throw new Exception("un handled model type : " + model_type.ToString());
			}
		}

		public static string getVerbosePlural(ModelType model_type)
		{
			switch (model_type)
			{
				case ModelType.PROJECT_MODEL:		return "projects";
				case ModelType.MODEL_CLIENT:		return "clients";
				case ModelType.MODEL_SUPPLIER:		return "suppliers";
				case ModelType.PROGRESS_CLIENT:		return "progress-client-files";
				case ModelType.PROGRESS_SUPPLIER:	return "progress-supplier-files";
				case ModelType.PROGRESS_PAYMENT:	return "";
				default: throw new Exception("un handled model type : " + model_type.ToString());
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
				case ModelType.MODEL_SUPPLIER: {
						foreach (var supplier in Application.singleton.getSuppliers()) {
							if (supplier.matchPK(pk)) return supplier;
						}
						throw new ModelNotExists("model for supplier not exists pk: " + pk.ToString());
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
