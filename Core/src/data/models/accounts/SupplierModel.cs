using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Data.Models
{
	[Serializable]
	public class SupplierModel : Model
	{
		public IntergerField id		{ get; set; } = new IntergerField(	name: "id", is_required: true );
		public TextField name		{ get; set; } = new TextField(		name: "name" );

		/* overrides */
		override public string ToString()			=> this.name.value;
		override public ModelType getType()			=> ModelType.MODEL_SUPPLIER;
		override public bool matchPK(object pk)		=> id.value == Convert.ToInt64(pk);
		override public object getPK()				=> id.value;
		override public void saveUpdates()			=> Application.singleton.suppliers_file.save();
		override public void saveNew() {
			Application.singleton.suppliers_file.data.suppliers.Add(this);
			Application.singleton.suppliers_file.save();
		}
		public override void validateRelation() { }

	}
}
