using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Core.Data.Models
{
	[Serializable]
	public class PaymentModel : IModelData
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
		public ModelDataType getType() => ModelDataType.PAYMENT;
	}

	[Serializable]
	public class SupplierProgressModel : IModelData
	{
		public FloatField		total_amount_tobe_paid	{ get; set; } = new FloatField();
		public ListField<int>	payments				{ get; set; } = new ListField<int>();
		public DateTimeField	production_start_date	{ get; set; } = new DateTimeField();
		public DateTimeField	estimated_arival_date	{ get; set; } = new DateTimeField();
		public DateTimeField	shipping_date			{ get; set; } = new DateTimeField();
		public DateTimeField	arrival_date			{ get; set; } = new DateTimeField();

		// TODO: text field for comments, and attachments

		public ModelDataType getType() => ModelDataType.PROGRESS_SUPPLIER;
	}

	[Serializable]
	public class ClientProgressModel : IModelData
	{
		public FloatField		total_amount_tobe_paid			{ get; set; } = new FloatField();
		public ListField<int>   payments						{ get; set; } = new ListField<int>();
		public DateTimeField	arrived_date					{ get; set; } = new DateTimeField();
		public FloatField		charge_for_late_picking			{ get; set; } = new FloatField();
		public DateTimeField	picked_up_date					{ get; set; } = new DateTimeField();
		public DateTimeField	unloaded_date					{ get; set; } = new DateTimeField();

		public DateTimeField mechanical_installation_start_date { get; set; } = new DateTimeField();
		public DateTimeField mechanical_installation_end_date	{ get; set; } = new DateTimeField();
		public DateTimeField electrical_installation_start_date { get; set; } = new DateTimeField();
		public DateTimeField electrical_installation_end_date	{ get; set; } = new DateTimeField();
		public BoolField	 is_three_phase_available			{ get; set; } = new BoolField();
		public DateTimeField three_pahse_checked_date			{ get; set; } = new DateTimeField();
		public DateTimeField test_start_date					{ get; set; } = new DateTimeField();
		public DateTimeField test_end_date						{ get; set; } = new DateTimeField();

		// TODO: text field for comments, and attachments

		public ModelDataType getType() => ModelDataType.PROGRESS_CLIENT;
	}



}
