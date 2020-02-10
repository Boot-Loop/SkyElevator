using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Core.src;

namespace Core.src.documents
{
    public enum FieldType
    {
        TEXT, INTEGER, BOOL, FLOAT, DATE_TIME, DIMENTION,
    }

	/* an interface for fields of all documents */
	public interface IField {
		string getName();
		bool isReadonly();
        string getReplaceTag();
        object getDefault();

        void setValue(object value, bool is_readonly = false);
        object getValue();
        FieldType getType();
	}



	/* implimentation of the fields */
	public class TextField : IField
	{
		string name		        = "";
		bool is_readonly        = false;
        string replace_tag      = "";
        string default_value    = null;

		string _value	        = null;
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
        public string getName()             => name;
		public bool isReadonly()            => is_readonly;
        public string getReplaceTag()       => replace_tag;
        public object getDefault()          => default_value;
		public override string ToString()   => this.value.ToString();
        public object getValue()            => this.value;
        public void setValue(object value, bool is_readonly = false) { this.value = (string)value; this.is_readonly = is_readonly; }
        public FieldType getType() => FieldType.TEXT;
    }

	public class IntergerField : IField 
	{
		string name         = "";
        bool is_readonly    = false;
		bool is_positive    = false;
        string replace_tag  = "";
        int default_value   = 0;

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
        public void setValue(object value, bool is_readonly = false) { this.value = (long)value; this.is_readonly = is_readonly; }
        public object getValue() { return this.value;  }
        public string getReplaceTag() => replace_tag;
        public object getDefault() => default_value;
        public FieldType getType() => FieldType.INTEGER;
    }

    public class BoolField : IField
    {
        string name         = "";
        bool is_readonly    = false;
        string replace_tag  = "";
        bool default_value  = false;

        bool _value = false;
        public bool value {
            get { return _value; }
            set {
                if (is_readonly) throw new ReadonlyError();
                this._value = value;
            }
        }

        public BoolField( string name, string replace_tag, bool read_only = false, bool default_value = false) {
            this.name = name; this.replace_tag = replace_tag; this.is_readonly = read_only; this.default_value = default_value;
        }
        public BoolField( string name, string replace_tag, bool value, bool read_only, bool default_value = false) {
            this.name = name; this.replace_tag = replace_tag; this.value = value; this.is_readonly = read_only; this.default_value = default_value;
        }

        public string getName()             => this.name;
        public bool isReadonly()            => this.is_readonly;
        public string getReplaceTag()       => this.replace_tag;
        public object getValue()            => this.value;
        public object getDefault()          => this.default_value;
        public void setValue(object value, bool is_readonly = false) { this.value = (bool)value; this.is_readonly = is_readonly; }
        override public string ToString()   => this.value.ToString();
        public FieldType getType() => FieldType.BOOL;
    }

	public class FloatField : IField 
	{
		string name             = "";
        bool is_readonly        = false;
		bool is_positive        = false;
        string replace_tag      = "";
        double default_value    = 0;

		double _value = 0.0d;
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
        public void setValue(object value, bool is_readonly = false) { this.value = (double)value; this.is_readonly = is_readonly; }
        public object getValue() => value;
        public FieldType getType() => FieldType.FLOAT;
    }

	public class DateTimeField : IField
	{
        public enum Format
        {
            DDSUP_MTXT_YYYY,
            MM_DD_YYYY,
        }

		string name = ""; bool is_readonly = false;
        string replace_tag = "";
        Format format = Format.MM_DD_YYYY;
        DateTime default_value = new DateTime();
        List<string> replace_tags;
        bool is_null = true;

        DateTime _value = new DateTime();
		public DateTime value {
			get { return _value; }
			set {
				if (is_readonly) throw new ReadonlyError();
                this._value = value;
                is_null = false;
			}
		}

		public DateTimeField(string name, string replace_tag, bool is_readonly = false, DateTime default_value = new DateTime(), Format format = Format.MM_DD_YYYY) {
			this.name = name; this.is_readonly = is_readonly; this.replace_tag = replace_tag; this.default_value = default_value; this.format = format;
		}
		public DateTimeField(string name, string replace_tag, DateTime datetime, bool is_readonly = false, DateTime default_value = new DateTime(), Format format = Format.MM_DD_YYYY) {
			this.name = name; this._value = datetime; this.is_readonly = is_readonly; this.format = format;
            this.replace_tag = replace_tag; this.default_value = default_value; is_null = false;
		}
        public DateTimeField(string name, List<string> replace_tags, bool is_readonly = false, DateTime default_value = new DateTime(), Format format = Format.DDSUP_MTXT_YYYY) {
			this.name = name; this.is_readonly = is_readonly; this.default_value = default_value; this.format = format;
            if (format != Format.DDSUP_MTXT_YYYY) throw new ArgumentException();
            setReplaceTags(replace_tags);
		}
		public DateTimeField(string name, List<string> replace_tags, DateTime datetime, bool is_readonly = false, DateTime default_value = new DateTime(), Format format = Format.DDSUP_MTXT_YYYY) {
			this.name = name; this._value = datetime; this.is_readonly = is_readonly; this.format = format; this.default_value = default_value;
            if (format != Format.DDSUP_MTXT_YYYY) throw new ArgumentException(); is_null = false;
            setReplaceTags(replace_tags);
		}

        public void setReplaceTags(List<string> replace_tags) {
            if (replace_tags.Count != 3) throw new ArgumentException("expected count is 3 for replace_tags");
            this.replace_tags = replace_tags;
        }
        public Dictionary<string, string> getReplaceTags() {
            if (this.replace_tags == null) throw new NullReferenceException("replace_tags is null");
            Dictionary<string, string> ret = new Dictionary<string, string>();
            ret.Add( this.replace_tags[0], this._value.Day.ToString());
            ret.Add( this.replace_tags[1], getDayPrefix());
            ret.Add( this.replace_tags[2], getMonthName() + " " + this._value.Year.ToString());
            return ret;
        }

		public string getName() => name;
		public bool isReadonly() => is_readonly;
        public override string ToString() {
            switch( this.format)
            {
                case Format.MM_DD_YYYY:
                    return this._value.ToString("MM/dd/yyyy");
                case Format.DDSUP_MTXT_YYYY: // unusable code from now;
                    return String.Format( "{0}{1} {2} {3}", this._value.Day.ToString(), getDayPrefix(), getMonthName(), this._value.Year );
                default:
                    return _value.ToString();
            }
        }
        public string getReplaceTag() => replace_tag;
        public object getDefault() => default_value;
        public void setValue(object value, bool is_readonly = false) { this.value = (DateTime)value; this.is_readonly = is_readonly; is_null = false; }
        public object getValue() => value;
        public FieldType getType() => FieldType.DATE_TIME;
        public Format getFormat() => this.format;
        public bool isNull() => is_null;

        private string getDayPrefix() {
            int day = this._value.Day;
            if (day % 10 == 1 && day != 11) return "st";
            if (day % 10 == 2 && day != 12) return "nd";
            if (day % 10 == 3 && day != 13) return "rd";
            return "th";
        }
        private string getMonthName() {
            switch ( this._value.Month) {
                case 1: return "January";
                case 2: return "February";
                case 3: return "March";
                case 4: return "April";
                case 5: return "May";
                case 6: return "June";
                case 7: return "July";
                case 8: return "August";
                case 9: return "September";
                case 10: return "October";
                case 11: return "November";
                case 12: return "December";
                default: return "<month unknown>";
            }
        }
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
		public string getName()             => name;
		public bool isReadonly()            => is_readonly;
		public double asMilliMeter()	    => _value;
		public double asCentiMeter()	    => _value / 10;
		public double asMeter()			    => _value / 1000;
		override public string ToString()   => _value.ToString() + " mm";
        public string getReplaceTag()       => replace_tag;
        public object getDefault()          => default_value;
        public void setValue(object value, bool is_readonly = false) { this.value = (double)value; this.is_readonly = is_readonly; }
        public object getValue()            => value;
        public FieldType getType() => FieldType.DIMENTION;
    }

	/* TODO: impliment other fields */
	//public class PhoneNumberField : IField { }
	//public class EmailField : IField { }
	//public class AddressField : IField { }
}


