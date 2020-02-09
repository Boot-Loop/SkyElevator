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
        string getReplaceTag();
        object getDefault();

        void setValue(object value);
        object getValue();
        
	}


	/* implimentation of the fields */
	public class TextField : IField
	{
		bool is_readonly        = false;
		string name		        = "";
		string _value	        = null;
        string replace_tag      = "";
        string default_value    = null;

		public string value { 
			get { return _value; }
			set {
				if (is_readonly) throw new ReadonlyError();
				this._value = value;
			}
		}
		public TextField(string name, string replace_tag, bool is_readonly = false, string default_value = null) {
            this.name = name; this.replace_tag = replace_tag;
            this.is_readonly = is_readonly; this.default_value = default_value;
        }
        public TextField(string name, string replace_tag, string text, bool is_readonly = false, string default_value = null) {
            this.name = name; this.replace_tag = replace_tag;
            this.value = text; this.is_readonly = is_readonly;  this.default_value = default_value;
        }
        public string getName() => name;
		public bool isReadonly() => is_readonly;
		public override string ToString() => value;
        public string getReplaceTag() => replace_tag;
        public object getDefault() => default_value;

        public void setValue(object value) {
            this.value = (string)value;
        }

        public object getValue() {
            return value;
        }
    }

	public class IntergerField : IField 
	{
		string name = ""; bool is_readonly = false;
		bool is_positive = false;
        string replace_tag = "";
        int default_value = 0;

        long _value = 0;
		public long value {
			get { return _value; }
			set {
				if (is_readonly) throw new ReadonlyError();
				if (is_positive && value < 0) throw new ArgumentException();
				this._value = value;
			}
		}

		public IntergerField(string name, string replace_tag, bool is_positive = false, bool is_readonly = false, int default_value = 0) {
			this.name = name; this.is_positive = is_positive; this.is_readonly = is_readonly;
            this.replace_tag = replace_tag;
            this.default_value = default_value;
		}
		public IntergerField(string name, string replace_tag, long value, bool is_positive = false, bool is_readonly = false, int default_value = 0) {
			this.name = name; this.is_positive = is_positive;
			this.value = value; this.is_readonly = is_readonly;
            this.replace_tag = replace_tag;
            this.default_value = default_value;
		}
		public string getName() => name;
		public bool isReadonly() => is_readonly;
		public override string ToString() => _value.ToString();
        public void setValue( object value ) { this.value = (long)value;  }
        public object getValue() { return this.value;  }
        public string getReplaceTag() => replace_tag;
        public object getDefault() => default_value;
    }

	public class FloatField : IField 
	{
		string name = ""; bool is_readonly = false;
		bool is_positive = false;
		double _value = 0.0d;
        string replace_tag = "";
        double default_value = 0;

        public double value {
			get { return _value; }
			set {
				if (is_readonly) throw new ReadonlyError();
				if (is_positive && value < 0) throw new ArgumentException();
				this._value = value;
			}
		}

		public FloatField(string name, string replace_tag, bool is_positive = false, bool is_readonly = false, double default_value = 0) {
			this.name = name; this.is_positive = is_positive; this.is_readonly = is_readonly;
            this.replace_tag = replace_tag; this.default_value = default_value;
		}
		public FloatField(string name, string replace_tag, double value, bool is_positive = false, bool is_readonly = false, double default_value = 0) {
			this.name = name; this.is_positive = is_positive;
			this.value = value; this.is_readonly = is_readonly;
            this.replace_tag = replace_tag; this.default_value = default_value;
		}
		public string getName() => name;
		public bool isReadonly() => is_readonly;
		public override string ToString() => _value.ToString();
        public string getReplaceTag() => replace_tag;
        public object getDefault() => default_value;
        public void setValue( object value) => this.value = (double)value;
        public object getValue() => value;
    }

	public class DateTimeField : IField
	{
		string name = ""; bool is_readonly = false;
        string replace_tag = "";
        DateTime default_value = new DateTime();


        DateTime _value = new DateTime();
		public DateTime value {
			get { return _value; }
			set {
				if (is_readonly) throw new ReadonlyError();
				/* TODO: validate and set value use the validator class */
			}
		}

		public DateTimeField(string name, string replace_tag, bool is_readonly = false, DateTime default_value = new DateTime()) {
			this.name = name; this.is_readonly = is_readonly; this.replace_tag = replace_tag; this.default_value = default_value;
		}
		public DateTimeField(string name, string replace_tag, DateTime datetime, bool is_readonly = false, DateTime default_value = new DateTime()) {
			this.name = name; this._value = datetime; this.is_readonly = is_readonly;
            this.replace_tag = replace_tag; this.default_value = default_value;
		}
		public string getName() => name;
		public bool isReadonly() => is_readonly;
		public override string ToString() => _value.ToString();
        public string getReplaceTag() => replace_tag;
        public object getDefault() => default_value;
        public void setValue(object value) => this.value = (DateTime)value;
        public object getValue() => value;
    }

	/// <summary>
	/// default dimention unit is mm, 1000 mm = 1 m
	/// </summary>
	public class DimensionField : IField 
	{
		string name = ""; bool is_readonly = false;
        string replace_tag = "";
        double default_value = 0;

        double _value;
		public double value { 
			get { return _value; }
			set {
				if (is_readonly) throw new ReadonlyError();
				_value = value;
			}
		}

		public DimensionField(string name, string replace_tag, bool is_readonly = false, double default_value = 0) {
			this.name = name; this.is_readonly = is_readonly; this.replace_tag = replace_tag; this.default_value = default_value;
		}
		public DimensionField(string name, string replace_tag, double value, bool is_readonly = false, double default_value = 0 ) {
			this.name = name; this.value = value; this.is_readonly = is_readonly; this.replace_tag = replace_tag; this.default_value = default_value;
		}
		public string getName() => name;
		public bool isReadonly() => is_readonly;
		public double asMilliMeter()	=> _value;
		public double asCentiMeter()	=> _value / 10;
		public double asMeter()			=> _value / 1000;
		override public string ToString() => _value.ToString() + " mm";
        public string getReplaceTag() => replace_tag;
        public object getDefault() => default_value;
        public void setValue(object value) => this.value = (double)value;
        public object getValue() => value;
    }

	/* TODO: impliment other fields */
	//public class PhoneNumberField : IField { }
	//public class EmailField : IField { }
	//public class AddressField : IField { }
}


