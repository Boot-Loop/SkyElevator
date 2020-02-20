using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Core.Data.Models
{
	[Serializable]
	public class PaymentModel : Model
	{
		public enum PaymentType {
			CASH			= 1,
			CHECK			= 2,
			CREDIT_CARD		= 3,
			DEBIT_CARD		= 4,
			BANK_DEPOSIT	= 5,
		}
		public IntergerField id { get; set; }			= new IntergerField();
		public FloatField amount { get; set; }			= new FloatField();
		public IntergerField payment_type { get; set; }	= new IntergerField();

		public void setPaymentType( PaymentType type) => payment_type.value = (int)type;
		override public ModelType getType() => ModelType.PROGRESS_PAYMENT;

		public override bool matchPK(object pk) {
			return id.value.Equals((int)pk);
		}
		public override object getPK(){
			return id.value;
		}
		
		public override void save() {
			if (ProjectManager.singleton.hasProgressTracking())
				ProjectManager.singleton.progress_payments.save();
			else throw new InvalidOperationException("project has no progress tracking");
		}

	}

	[Serializable]
	public class SupplierProgressModel : Model
	{
		public IntergerField	id						{ get; set; } = new IntergerField(	name: "id"						);
		public FloatField		total_amount_tobe_paid	{ get; set; } = new FloatField(		name: "total_ammount"			);
		public ListField<int>	payments				{ get; set; } = new ListField<int>(	name: "payments"				);
		public DateTimeField	production_start_date	{ get; set; } = new DateTimeField(	name: "production_start_date"	);
		public DateTimeField	estimated_arival_date	{ get; set; } = new DateTimeField(	name: "estimated_arival_date"	);
		public DateTimeField	shipping_date			{ get; set; } = new DateTimeField(	name: "shipping_date"			);
		public DateTimeField	arrival_date			{ get; set; } = new DateTimeField(	name: "arrival_date"			);

		// TODO: text field for comments, and attachments

		override public ModelType getType() => ModelType.PROGRESS_SUPPLIER;

		public override bool matchPK(object pk) {
			return id.value.Equals((int)pk);
		}
		public override object getPK() {
			return id.value;
		}
		public override void save() {
			if (ProjectManager.singleton.hasProgressTracking())
				ProjectManager.singleton.progress_supplier.save();
			else throw new InvalidOperationException("project has no progress tracking");
		}
	}

	[Serializable]
	public class ClientProgressModel : Model
	{
		public IntergerField	id								{ get; set; } = new IntergerField(	name: "id"						);
		public FloatField		total_amount_tobe_paid			{ get; set; } = new FloatField(		name: "total_ammount"			);
		public ListField<int>   payments						{ get; set; } = new ListField<int>(	name: "payments"				);
		public DateTimeField	arrived_date					{ get; set; } = new DateTimeField(	name: "arrival_date"			);
		public FloatField		charge_for_late_picking			{ get; set; } = new FloatField(		name: "charge_for_late_picking" );
		public DateTimeField	picked_up_date					{ get; set; } = new DateTimeField(	name: "picked_up_date"			);
		public DateTimeField	unloaded_date					{ get; set; } = new DateTimeField(	name: "unloaded_date"			);
																									
		public DateTimeField mechanical_installation_start_date { get; set; } = new DateTimeField(	name: "inst_mech_start_date"	);
		public DateTimeField mechanical_installation_end_date	{ get; set; } = new DateTimeField(	name: "inst_mech_end_date"		);
		public DateTimeField electrical_installation_start_date { get; set; } = new DateTimeField(	name: "inst_elec_start_date"	);
		public DateTimeField electrical_installation_end_date	{ get; set; } = new DateTimeField(	name: "inst_elec_end_date"		);
		public BoolField	 is_three_phase_available			{ get; set; } = new BoolField(		name: "is_three_pahse_avail"	);
		public DateTimeField three_pahse_checked_date			{ get; set; } = new DateTimeField(	name: "three_pahse_check_date"	);
		public DateTimeField test_start_date					{ get; set; } = new DateTimeField(	name: "test_start_date"			);
		public DateTimeField test_end_date						{ get; set; } = new DateTimeField(	name: "test_end_date"			);

		// TODO: text field for comments, and attachments

		override public ModelType getType() => ModelType.PROGRESS_CLIENT;

		public override bool matchPK(object pk) {
			return id.value.Equals((int)pk);
		}
		public override object getPK() {
			return id.value;
		}

		public override void save() {
			if (ProjectManager.singleton.hasProgressTracking())
				ProjectManager.singleton.progress_client.save();
			else throw new InvalidOperationException("project has no progress tracking");
		}
	}



}
