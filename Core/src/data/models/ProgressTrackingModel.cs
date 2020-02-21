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

		public void setPaymentType( PaymentType type)	=> payment_type.value = (int)type;
		/* overrides */
		override public ModelType getType()				=> ModelType.PROGRESS_PAYMENT;
		public override bool matchPK(object pk)			=> id.value == Convert.ToInt64(pk);
		public override object getPK()					=> id.value;

		public override void saveUpdates() {
			if (ProjectManager.singleton.hasProgressTracking())
				ProjectManager.singleton.progress_payments.save();
			else throw new InvalidOperationException("project has no progress tracking");
		}
		public override void saveNew() {
			ProjectManager.singleton.progress_payments.data.Add(this);
			ProjectManager.singleton.progress_payments.save();
		}
		public override void validateRelation() { }
	}

	

	[Serializable]
	public class ClientProgressModel : Model
	{
		public IntergerField	id								{ get; set; } = new IntergerField(	name: "id",			is_required: true	);
		public IntergerField	project_id						{ get; set; } = new IntergerField(	name: "project_id", is_required: true	);
		public FloatField		total_amount_tobe_paid			{ get; set; } = new FloatField(		name: "total_ammount"					);
		public ListField<int>   payments						{ get; set; } = new ListField<int>(	name: "payments"						);
		public DateTimeField	arrived_date					{ get; set; } = new DateTimeField(	name: "arrival_date"					);
		public FloatField		charge_for_late_picking			{ get; set; } = new FloatField(		name: "charge_for_late_picking"			);
		public DateTimeField	picked_up_date					{ get; set; } = new DateTimeField(	name: "picked_up_date"					);
		public DateTimeField	unloaded_date					{ get; set; } = new DateTimeField(	name: "unloaded_date"					);
																									
		public DateTimeField mechanical_installation_start_date { get; set; } = new DateTimeField(	name: "inst_mech_start_date"			);
		public DateTimeField mechanical_installation_end_date	{ get; set; } = new DateTimeField(	name: "inst_mech_end_date"				);
		public DateTimeField electrical_installation_start_date { get; set; } = new DateTimeField(	name: "inst_elec_start_date"			);
		public DateTimeField electrical_installation_end_date	{ get; set; } = new DateTimeField(	name: "inst_elec_end_date"				);
		public BoolField	 is_three_phase_available			{ get; set; } = new BoolField(		name: "is_three_pahse_avail"			);
		public DateTimeField three_pahse_checked_date			{ get; set; } = new DateTimeField(	name: "three_pahse_check_date"			);
		public DateTimeField test_start_date					{ get; set; } = new DateTimeField(	name: "test_start_date"					);
		public DateTimeField test_end_date						{ get; set; } = new DateTimeField(	name: "test_end_date"					);

		// TODO: text field for comments, and attachments

		/* overrides */
		override public ModelType getType() => ModelType.PROGRESS_CLIENT;
		public override bool matchPK(object pk) => id.value == Convert.ToInt64(pk);
		public override object getPK() => id.value;
		public override void saveUpdates() {
			if (ProjectManager.singleton.hasProgressTracking())
				ProjectManager.singleton.progress_client.save();
			else throw new InvalidOperationException("project has no progress tracking");
		}
		public override void saveNew() {
			if (Application.singleton.is_proj_loaded) throw new InvalidOperationException("can't create project when another project already loaded");
			var progress_client_file = ProjectManager.singleton.progress_client;
			if (progress_client_file.path is null) throw new Exception("path is null -> did you call Application.createNewProject()");
			progress_client_file.data = this;
			progress_client_file.save();
		}

		public override void validateRelation() {
			if (ProjectManager.singleton.project_file.data.project_model.id.value != project_id.value) {
				throw new ModelNotExists("project id didn't match with client progress model's fk");
			}
		}
	}

	[Serializable]
	public class SupplierProgressModel : Model
	{
		public IntergerField id						{ get; set; } = new IntergerField(	name: "id",			is_required: true	);
		public IntergerField project_id				{ get; set; } = new IntergerField(	name: "project_id", is_required: true	);
		public FloatField total_amount_tobe_paid	{ get; set; } = new FloatField(		name: "total_ammount"					);
		public ListField<int> payments				{ get; set; } = new ListField<int>(	name: "payments"						);
		public DateTimeField production_start_date	{ get; set; } = new DateTimeField(	name: "production_start_date"			);
		public DateTimeField estimated_arival_date	{ get; set; } = new DateTimeField(	name: "estimated_arival_date"			);
		public DateTimeField shipping_date			{ get; set; } = new DateTimeField(	name: "shipping_date"					);
		public DateTimeField arrival_date			{ get; set; } = new DateTimeField(	name: "arrival_date"					);

		// TODO: text field for comments, and attachments

		/* overrides */
		override public ModelType getType() => ModelType.PROGRESS_SUPPLIER;
		public override bool matchPK(object pk) => id.value == Convert.ToInt64(pk);
		public override object getPK() => id.value;
		public override void saveUpdates() {
			if (ProjectManager.singleton.hasProgressTracking())
				ProjectManager.singleton.progress_supplier.save();
			else throw new InvalidOperationException("project has no progress tracking");
		}
		public override void saveNew() {
			if (Application.singleton.is_proj_loaded) throw new InvalidOperationException("can't create project when another project already loaded");
			var progress_supplier_file = ProjectManager.singleton.progress_supplier;
			if (progress_supplier_file.path is null) throw new Exception("path is null -> did you call Application.createNewProject()");
			progress_supplier_file.data = this;
			progress_supplier_file.save();
		}
		public override void validateRelation() {
			if (ProjectManager.singleton.project_file.data.project_model.id.value != project_id.value) {
				throw new ModelNotExists("project id didn't match with supplier progress model's fk");
			}
			foreach( var payment_id in payments.value) {
				Model.getModel(payment_id, ModelType.PROGRESS_PAYMENT);
			}
		}
	}



}
