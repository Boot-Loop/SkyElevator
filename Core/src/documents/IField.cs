using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Core.src;

namespace Core.src.documents
{
	/* an interface for fields of all documents */
	public interface IField {
		string getName();
		bool isReadonly();
	}


	/* implimentation of the fields */
	public class TextField : IField
	{
		bool is_readonly = false;
		string name		= "";
		string _value	= "";
		public string value { 
			get { return _value; }
			set {
				if (is_readonly) throw new ReadonlyError();
				this._value = value;
			}
		}
		public TextField(string name, bool is_readonly) { this.name = name; this.is_readonly = is_readonly; }
		public TextField(string name, string text) { this.name = name; this.value = text; }
		public string getName() => name;
		public bool isReadonly() => is_readonly;
		public override string ToString() => value;

	}

	public class IntergerField : IField 
	{
		string name = ""; bool is_readonly = false;
		bool is_positive = false;
		long _value = 0;
		public long value {
			get { return _value; }
			set {
				if (is_readonly) throw new ReadonlyError();
				if (is_positive && value < 0) throw new ArgumentException();
				this._value = value;
			}
		}

		public IntergerField(string name, bool is_positive = false, bool is_readonly = false) {
			this.name = name; this.is_positive = is_positive; this.is_readonly = is_readonly;
		}
		public IntergerField(string name, long value, bool is_positive = false, bool is_readonly = false) {
			this.name = name; this.is_positive = is_positive;
			this.value = value; this.is_readonly = is_readonly;
		}
		public string getName() => name;
		public bool isReadonly() => is_readonly;
		public override string ToString() => _value.ToString();
	}

	public class FloatField : IField 
	{
		string name = ""; bool is_readonly = false;
		bool is_positive = false;
		double _value = 0.0d;
		public double value {
			get { return _value; }
			set {
				if (is_readonly) throw new ReadonlyError();
				if (is_positive && value < 0) throw new ArgumentException();
				this._value = value;
			}
		}

		public FloatField(string name, bool is_positive = false, bool is_readonly = false) {
			this.name = name; this.is_positive = is_positive; this.is_readonly = is_readonly;
		}
		public FloatField(string name, double value, bool is_positive = false, bool is_readonly = false) {
			this.name = name; this.is_positive = is_positive;
			this.value = value; this.is_readonly = is_readonly;
		}
		public string getName() => name;
		public bool isReadonly() => is_readonly;
		public override string ToString() => _value.ToString();
	}

	public class DateTimeField : IField
	{
		string name = ""; bool is_readonly = false;
		DateTime _value = new DateTime();
		public DateTime value {
			get { return _value; }
			set {
				if (is_readonly) throw new ReadonlyError();
				/* TODO: validate and set value use the validator class */
			}
		}

		public DateTimeField(string name, bool is_readonly = false) {
			this.name = name; this.is_readonly = is_readonly;
		}
		public DateTimeField(string name, DateTime datetime, bool is_readonly = false) {
			this.name = name; this._value = datetime; this.is_readonly = is_readonly;
		}
		public string getName() => name;
		public bool isReadonly() => is_readonly;
		public override string ToString() => _value.ToString();
	}

	/// <summary>
	/// default dimention unit is mm, 1000 mm = 1 m
	/// </summary>
	public class DimensionField : IField 
	{
		string name = ""; bool is_readonly = false;
		double _value;
		public double value { 
			get { return _value; }
			set {
				if (is_readonly) throw new ReadonlyError();
				_value = value;
			}
		}

		public DimensionField(string name, bool is_readonly = false) {
			this.name = name; this.is_readonly = is_readonly;
		}
		public DimensionField(string name, double value, bool is_readonly = false) {
			this.name = name; this.value = value; this.is_readonly = is_readonly;
		}
		public string getName() => name;
		public bool isReadonly() => is_readonly;
		public double asMilliMeter()	=> _value;
		public double asCentiMeter()	=> _value / 10;
		public double asMeter()			=> _value / 1000;
		override public string ToString() => _value.ToString() + " mm";
	}

	/* TODO: impliment other fields */
	//public class PhoneNumberField : IField { }
	//public class EmailField : IField { }
	//public class AddressField : IField { }
}


