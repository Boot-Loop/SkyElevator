using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

using Core.Data.Models;

namespace Core.Data.Files
{
	[Serializable]
	public class ClientsData
	{
		public ObservableCollection<ClientModel> clients = new ObservableCollection<ClientModel>();
	}
}
