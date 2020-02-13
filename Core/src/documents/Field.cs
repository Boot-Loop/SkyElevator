using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

using Core.src;
using Core.utils;

namespace Core.src.documents
{
    public enum FieldType
    {
        TEXT, INTEGER, BOOL, FLOAT, DATE_TIME, DIMENTION, EMAIL, PHONE_NUMBER, NIC, WEB_SITE,
        DROP_DOWN,
    }

    public abstract class Field
    {
        protected string name;
        protected string replace_tag;
        protected bool is_readonly              = false;
        protected bool is_required              = false;
        protected bool is_null                  = true;

        protected object last_value             = null; // value before validation
        protected bool last_value_valid         = true;
        protected string validation_error_msg   = "";

        public string getName()  => name;
        public bool isReadonly() => is_readonly;

        abstract public object      getDefault();
        abstract public void        setValue(object value, bool is_readonly = false);
        abstract public object      getValue();
        abstract public FieldType   getType();

        public string getReplaceTag() => replace_tag;
        public void setRequired(bool is_required) => this.is_required = is_required;
        public bool isRequired() => is_required;
        public bool isNull() => is_null;
        public void setValidationErrorMsg(string msg) => validation_error_msg = msg;
        public string getValidationErrorMsg() { 
            if (last_value_valid) return ""; 
            return validation_error_msg; 
        }
        public object getLastValue() => last_value;
    }


	public class TextField : Field
	{
		
        string default_value    = null;
		string _value	        = null;
        [XmlAttribute]
		public string value { 
			get { return _value; }
			set {
				if (is_readonly) throw new ReadonlyError();
                last_value = value; 
                last_value_valid = false;
                _validate(value); 
                last_value_valid = true;
				this._value = value;
                is_null = false;
			}
		}
        private TextField() { }
        public TextField(bool is_readonly = false, string default_value = null, bool is_required = false, string validation_error_msg = "" ) { 
            this.is_readonly = is_readonly; this.default_value = default_value; this.is_required = is_required;  this.validation_error_msg = validation_error_msg;
        }
        public TextField( string value, bool is_readonly = false, string default_value = null, bool is_required = false, string validation_error_msg = "") { 
            this.value = value; this.is_readonly = is_readonly; this.default_value = default_value; this.is_required = is_required; this.validation_error_msg = validation_error_msg;
        }
        /* constructor for document */
		public TextField(string name, string replace_tag, bool is_readonly = false, string default_value = null, bool is_required = false, string validation_error_msg = "") {
            this.name = name; this.replace_tag = replace_tag; this.is_readonly = is_readonly; 
            this.default_value = default_value; this.is_required = is_required; this.validation_error_msg = validation_error_msg;
        }
        public TextField(string name, string replace_tag, string text, bool is_readonly = false, string default_value = null, bool is_required = false, string validation_error_msg = "") {
            this.name = name; this.replace_tag = replace_tag; this.value = text; this.is_readonly = is_readonly;  
            this.default_value = default_value; this.is_required = is_required; this.validation_error_msg = validation_error_msg;
        }
        override public FieldType getType() => FieldType.TEXT;
        override public object getDefault() => default_value;
        override public string ToString()   => this.value.ToString();
        override public object getValue()   => this.value;
        override public void setValue(object value, bool is_readonly = false) { this.value = (string)value; this.is_readonly = is_readonly; }
        virtual public void _validate(string value) { }
    }

    public class BoolField : Field
    {
        
        bool default_value  = false;
        bool _value = false;
        [XmlAttribute]
        public bool value {
            get { return _value; }
            set {
                if (is_readonly) throw new ReadonlyError();
                last_value = value;
                this._value = value;
                is_null = false;
            }
        }

        private BoolField() { }
        public BoolField(bool value, bool is_readonly = false, bool default_value = false, string validation_error_msg = "") { 
            this.value = value; this.is_readonly = is_readonly; this.default_value = default_value; this.validation_error_msg = validation_error_msg;
        }

        /* constructor for document */
        public BoolField(string name, string replace_tag, bool read_only = false, bool default_value = false, string validation_error_msg = "") {
            this.name = name; this.replace_tag = replace_tag; this.is_readonly = read_only; 
            this.default_value = default_value; this.validation_error_msg = validation_error_msg;
        }
        public BoolField(string name, string replace_tag, bool value, bool read_only, bool default_value = false, string validation_error_msg = "") {
            this.name = name; this.replace_tag = replace_tag; this.value = value; this.is_readonly = read_only;
            this.default_value = default_value; this.validation_error_msg = validation_error_msg;
        }

        override public FieldType getType() => FieldType.BOOL;
        override public object getValue()   => this.value;
        override public object getDefault() => this.default_value;
        override public void setValue(object value, bool is_readonly = false) { this.value = (bool)value; this.is_readonly = is_readonly; }
        override public string ToString() => this.value.ToString();
    }


    public class IntergerField : Field 
	{
		bool is_positive    = false;
        long default_value   = 0;

        long _value = 0;
        [XmlAttribute]
		public long value {
			get { return _value; }
			set {
				if (is_readonly) throw new ReadonlyError();
                last_value = value;
                last_value_valid = false;
				if (is_positive && value < 0) throw new ArgumentException();
                last_value_valid = true;
				this._value = value;
                is_null = false;
			}
		}

        private IntergerField() { }
        public IntergerField(bool is_positive = false, bool is_readonly = false, long default_value = 0, bool is_required = false, string validation_error_msg = "") {
            this.is_positive = is_positive; this.is_readonly = is_readonly; 
            this.default_value = default_value; this.is_required = is_required; this.validation_error_msg = validation_error_msg;
        }
        public IntergerField(long value, bool is_positive = false, bool is_readonly = false, long default_value = 0, bool is_required = false, string validation_error_msg = "") {
            this.value = value; this.is_positive = is_positive; this.is_readonly = is_readonly; 
            this.default_value = default_value; this.is_required = is_required; this.validation_error_msg = validation_error_msg;
        }

        /* constructor for document */
        public IntergerField(string name, string replace_tag, bool is_positive = false, bool is_readonly = false, long default_value = 0, bool is_required = false, string validation_error_msg = "") {
			this.name = name; this.is_positive = is_positive; this.is_readonly = is_readonly; this.replace_tag = replace_tag;
            this.default_value = default_value; this.is_required = is_required; this.validation_error_msg = validation_error_msg;
        }
		public IntergerField(string name, string replace_tag, long value, bool is_positive = false, bool is_readonly = false, long default_value = 0, bool is_required = false, string validation_error_msg = "") {
			this.name = name; this.is_positive = is_positive; this.value = value; this.is_readonly = is_readonly;
            this.replace_tag = replace_tag; this.default_value = default_value; this.is_required = is_required; this.validation_error_msg = validation_error_msg;
        }
        override public FieldType getType() => FieldType.INTEGER;
		override public string ToString() => _value.ToString();
        override public void setValue(object value, bool is_readonly = false) { this.value = (long)value; this.is_readonly = is_readonly; }
        override public object getValue() { return this.value;  }
        override public object getDefault() => default_value;
        public bool isPositive() => is_positive;
    }

    public class FloatField : Field 
	{
		bool is_positive        = false;
        double default_value    = 0;

		double _value = 0.0d;
        [XmlAttribute]
        public double value {
			get { return _value; }
			set {
				if (is_readonly) throw new ReadonlyError();
                last_value = value;
                last_value_valid = false;
				if (is_positive && value < 0) throw new ArgumentException();
                last_value_valid = true;
				this._value = value;
                is_null = false;
			}
		}

        private FloatField() { }
        public FloatField(bool is_positive = false, bool is_readonly = false, double default_value = 0, bool is_required = false, string validation_error_msg = "") { 
            this.is_positive = is_positive; this.is_readonly = is_readonly; 
            this.default_value = default_value; this.is_required = is_required; this.validation_error_msg = validation_error_msg;
        }
        public FloatField( double value, bool is_positive = false, bool is_readonly = false, double default_value = 0, bool is_required = false, string validation_error_msg = "") { 
            this.value = value; this.is_positive = is_positive; this.is_readonly = is_readonly; 
            this.default_value = default_value; this.is_required = is_required; this.validation_error_msg = validation_error_msg;
        }

        /* constructor for document */
		public FloatField(string name, string replace_tag, bool is_positive = false, bool is_readonly = false, double default_value = 0, bool is_required = false, string validation_error_msg = "") {
			this.name = name; this.is_positive = is_positive; this.is_readonly = is_readonly;
            this.replace_tag = replace_tag; this.default_value = default_value; this.is_required = is_required; this.validation_error_msg = validation_error_msg;
        }
		public FloatField(string name, string replace_tag, double value, bool is_positive = false, bool is_readonly = false, double default_value = 0, bool is_required = false, string validation_error_msg = "") {
			this.name = name; this.is_positive = is_positive; this.value = value; this.is_readonly = is_readonly;
            this.replace_tag = replace_tag; this.default_value = default_value; this.is_required = is_required; this.validation_error_msg = validation_error_msg;
        }
        override public FieldType getType() => FieldType.FLOAT;
		override public string ToString() => _value.ToString();
        override public object getDefault() => default_value;
        override public void setValue(object value, bool is_readonly = false) { this.value = (double)value; this.is_readonly = is_readonly; }
        override public object getValue() => value;
        public bool isPositive() => is_positive;
    }

	public class DateTimeField : Field
	{
        public enum Format
        {
            DDSUP_MTXT_YYYY,
            MTXT_D_YYYY,
            MM_DD_YYYY,
        }

        List<string> replace_tags;
        Format format           = Format.MM_DD_YYYY;
        DateTime default_value  = new DateTime();

        DateTime _value = new DateTime();
        [XmlAttribute]
		public DateTime value {
			get { return _value; }
			set {
				if (is_readonly) throw new ReadonlyError();
                last_value = value;
                this._value = value;
                is_null = false;
			}
		}

        private DateTimeField() { }
        public DateTimeField(bool is_readonly = false, DateTime default_value = new DateTime(), bool is_required = false, string validation_error_msg = "") { 
            this.is_readonly = is_readonly; this.default_value = default_value; this.is_required = is_required; this.validation_error_msg = validation_error_msg;
        }
        public DateTimeField( DateTime value, bool is_readonly = false, DateTime default_value = new DateTime(), bool is_required = false, string validation_error_msg = "") { 
            this.value = value; this.is_readonly = is_readonly; this.default_value = default_value; this.is_required = is_required; this.validation_error_msg = validation_error_msg;
        }

        /* constructor for document */
		public DateTimeField(string name, string replace_tag, bool is_readonly = false, DateTime default_value = new DateTime(), Format format = Format.MM_DD_YYYY, bool is_required = false, string validation_error_msg = "") {
			this.name = name; this.is_readonly = is_readonly; this.replace_tag = replace_tag; 
            this.default_value = default_value; this.format = format; this.is_required = is_required; this.validation_error_msg = validation_error_msg;
        }
		public DateTimeField(string name, string replace_tag, DateTime datetime, bool is_readonly = false, DateTime default_value = new DateTime(), Format format = Format.MM_DD_YYYY, bool is_required = false, string validation_error_msg = "") {
			this.name = name; this._value = datetime; this.is_readonly = is_readonly; this.format = format;
            this.replace_tag = replace_tag; this.default_value = default_value; is_null = false; this.is_required = is_required; this.validation_error_msg = validation_error_msg;
        }
        public DateTimeField(string name, List<string> replace_tags, bool is_readonly = false, DateTime default_value = new DateTime(), Format format = Format.DDSUP_MTXT_YYYY, bool is_required = false, string validation_error_msg = "") {
			this.name = name; this.is_readonly = is_readonly; this.default_value = default_value; 
            this.format = format; this.is_required = is_required; this.validation_error_msg = validation_error_msg;
            if (format != Format.DDSUP_MTXT_YYYY) throw new ArgumentException();
            setReplaceTags(replace_tags);
		}
		public DateTimeField(string name, List<string> replace_tags, DateTime datetime, bool is_readonly = false, DateTime default_value = new DateTime(), Format format = Format.DDSUP_MTXT_YYYY, bool is_required = false) {
			this.name = name; this._value = datetime; this.is_readonly = is_readonly; this.format = format; 
            this.default_value = default_value; this.is_required = is_required; this.validation_error_msg = validation_error_msg;
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


        override public string ToString() {
            switch( this.format)
            {
                case Format.MM_DD_YYYY:
                    return this._value.ToString("MM/dd/yyyy");
                case Format.MTXT_D_YYYY:
                    return String.Format("{0} {1}, {2}", getMonthName(), this._value.Day.ToString(), this._value.Year);
                case Format.DDSUP_MTXT_YYYY: // unusable code from now;
                    return String.Format( "{0}{1} {2} {3}", this._value.Day.ToString(), getDayPrefix(), getMonthName(), this._value.Year );
                default:
                    return _value.ToString();
            }
        }
        override public FieldType getType() => FieldType.DATE_TIME;
        override public object getDefault() => default_value;
        override public void setValue(object value, bool is_readonly = false) { this.value = (DateTime)value; this.is_readonly = is_readonly; is_null = false; }
        override public object getValue() => value;
        public Format getFormat() => this.format;


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

    public class EmailField : TextField
    {
        private EmailField() { }
        public EmailField(bool is_readonly = false, string default_value = null, bool is_required = false, string validation_error_msg = "") : base(is_readonly : is_readonly, default_value : default_value, is_required : is_required, validation_error_msg : validation_error_msg) { }
        public EmailField(string value, bool is_readonly = false, string default_value = null, bool is_required = false, string validation_error_msg = "") : base(value: value, is_readonly : is_readonly, default_value : default_value, is_required : is_required, validation_error_msg : validation_error_msg) { }
        /* constructor for document */
        public EmailField(string name, string replace_tag, bool is_readonly = false, string default_value = null, bool is_required = false, string validation_error_msg = "")
        :base(name: name, replace_tag: replace_tag, is_readonly : is_readonly, default_value : default_value, is_required : is_required, validation_error_msg:validation_error_msg) { }
        public EmailField(string name, string replace_tag, string text, bool is_readonly = false, string default_value = null, bool is_required = false, string validation_error_msg = "")
        :base (name: name, replace_tag: replace_tag, text: text, is_readonly : is_readonly, default_value : default_value, is_required : is_required, validation_error_msg: validation_error_msg) { }

        override public FieldType getType() => FieldType.EMAIL;
        public override void _validate(string value) {
            if (!Validator.validateEmail(value)) throw new ValidationError("invalide email");
        }
    }

    public class PhoneNumberField : TextField
    {
        private PhoneNumberField() { }
        public PhoneNumberField(bool is_readonly = false, string default_value = null, bool is_required = false, string validation_error_msg = "") : base(is_readonly: is_readonly, default_value: default_value, is_required: is_required, validation_error_msg: validation_error_msg) { }
        public PhoneNumberField(string value, bool is_readonly = false, string default_value = null, bool is_required = false, string validation_error_msg = "") : base(value: value, is_readonly: is_readonly, default_value: default_value, is_required: is_required, validation_error_msg: validation_error_msg) { }
        /* constructor for document */
        public PhoneNumberField(string name, string replace_tag, bool is_readonly = false, string default_value = null, bool is_required = false, string validation_error_msg = "")
        : base(name: name, replace_tag: replace_tag, is_readonly: is_readonly, default_value: default_value, is_required: is_required, validation_error_msg: validation_error_msg) { }
        public PhoneNumberField(string name, string replace_tag, string text, bool is_readonly = false, string default_value = null, bool is_required = false, string validation_error_msg = "")
        : base(name: name, replace_tag: replace_tag, text: text, is_readonly: is_readonly, default_value: default_value, is_required: is_required, validation_error_msg: validation_error_msg) { }

        override public FieldType getType() => FieldType.PHONE_NUMBER;
        public override void _validate(string value) {
            // todo:
        }
    }

    public class NICField : TextField
    {
        private NICField() { }
        public NICField(bool is_readonly = false, string default_value = null, bool is_required = false, string validation_error_msg = "") : base(is_readonly: is_readonly, default_value: default_value, is_required: is_required, validation_error_msg: validation_error_msg) { }
        public NICField(string value, bool is_readonly = false, string default_value = null, bool is_required = false, string validation_error_msg = "") : base(value: value, is_readonly: is_readonly, default_value: default_value, is_required: is_required, validation_error_msg: validation_error_msg) { }
        /* constructor for document */
        public NICField(string name, string replace_tag, bool is_readonly = false, string default_value = null, bool is_required = false, string validation_error_msg = "")
        : base(name: name, replace_tag: replace_tag, is_readonly: is_readonly, default_value: default_value, is_required: is_required, validation_error_msg: validation_error_msg) { }
        public NICField(string name, string replace_tag, string text, bool is_readonly = false, string default_value = null, bool is_required = false, string validation_error_msg = "")
        : base(name: name, replace_tag: replace_tag, text: text, is_readonly: is_readonly, default_value: default_value, is_required: is_required, validation_error_msg: validation_error_msg) { }

        override public FieldType getType() => FieldType.NIC;
        public override void _validate(string value) {
            // todo:
        }
    }
    
    public class WebSiteField : TextField
    {
        private WebSiteField() { }
        public WebSiteField(bool is_readonly = false, string default_value = null, bool is_required = false, string validation_error_msg = "") : base(is_readonly: is_readonly, default_value: default_value, is_required: is_required, validation_error_msg: validation_error_msg) { }
        public WebSiteField(string value, bool is_readonly = false, string default_value = null, bool is_required = false, string validation_error_msg = "") : base(value: value, is_readonly: is_readonly, default_value: default_value, is_required: is_required, validation_error_msg: validation_error_msg) { }
        /* constructor for document */
        public WebSiteField(string name, string replace_tag, bool is_readonly = false, string default_value = null, bool is_required = false, string validation_error_msg = "")
        : base(name: name, replace_tag: replace_tag, is_readonly: is_readonly, default_value: default_value, is_required: is_required, validation_error_msg: validation_error_msg) { }
        public WebSiteField(string name, string replace_tag, string text, bool is_readonly = false, string default_value = null, bool is_required = false, string validation_error_msg = "")
        : base(name: name, replace_tag: replace_tag, text: text, is_readonly: is_readonly, default_value: default_value, is_required: is_required, validation_error_msg: validation_error_msg) { }

        override public FieldType getType() => FieldType.WEB_SITE;
        public override void _validate(string value) {
            // todo:
        }
    }

    public class DropDownField<T> : Field
    {
        List<T> items = new List<T>();
        T _value;
        T default_value;
        public T value {
            get { return _value; }
            set {
                if (is_readonly) throw new ReadonlyError();
                last_value = value;
                this._value = value;
                is_null = false;
            }
        }

        private DropDownField() { }
        public DropDownField(bool is_readonly = false, T default_value = default(T), bool is_required = false, string validation_error_msg = "") {
            this.is_readonly = is_readonly; this.default_value = default_value; this.is_readonly = is_readonly; 
            this.validation_error_msg = validation_error_msg;
        }
        public DropDownField(List<T> items, bool is_readonly = false, T default_value = default(T), bool is_required = false, string validation_error_msg = "") {
            this.items = items; this.is_readonly = is_readonly; this.default_value = default_value; this.is_readonly = is_readonly; 
            this.validation_error_msg = validation_error_msg;
        }

        public override object getDefault() => default_value;
        public override FieldType getType() => FieldType.DROP_DOWN;
        public override object getValue() => value;
        public override void setValue(object value, bool is_readonly = false) { throw new NotSupportedException(); }
        public void addItem(T item) { items.Add(item); }
        public void setItem(int index) { this.value = items[index]; } // throws exception if index is out of range
        public void setItems(List<T> items) { this.items = items; }
        public List<T> getItems() => items;
    }
    


}
