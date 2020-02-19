using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Core.Data.Models;

namespace Core.Data.ModelAPI
{
	public class ModelAPI<T> where T : Model
	{
		public T model { get; set; } = default(T);

		public ModelAPI() { }
		public ModelAPI( T model ) => this.model = model;

	}
}
