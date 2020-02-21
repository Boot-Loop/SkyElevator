using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Core.Data.Models;

namespace Core.Data.Files
{
	[Serializable]
	public class SuppliersData
	{
		public List<SupplierModel> suppliers = new List<SupplierModel>();
	}
}
