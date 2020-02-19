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
	}
}
