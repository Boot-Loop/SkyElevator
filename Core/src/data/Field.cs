using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

using Core;
using Core.Utils;

namespace Core.Data
{
    public enum FieldType
    {
        TEXT, INTEGER, BOOL, FLOAT, DATE_TIME, DIMENTION, EMAIL, PHONE_NUMBER, NIC, WEB_SITE,
        DROP_DOWN,
    }

    [Serializable]
    public abstract class Field
    {
        protected string name;
        protected string replace_tag;
        protected bool is_readonly = false;
        protected bool is_required = false;
        protected bool is_null = true;

        protected object last_value = null; // value before validation
        protected bool last_value_valid = true;
        protected string validation_error_msg = "";

        public string getName() => name;
        public bool isReadonly() => is_readonly;

        abstract public object getDefault();
        abstract public void setValue(object value, bool is_readonly = false);
        abstract public object getValue();
        abstract public FieldType getType();

        public string getReplaceTag() => replace_tag;
        public void setRequired(bool is_required) => this.is_required = is_required;
        public bool isRequired() => is_required;
        public bool isNull() => is_null;
        public void setToNull() { this.is_null = true; }
        public void setValidationErrorMsg(string msg) => validation_error_msg = msg;
        public string getValidationErrorMsg() { 
            if (last_value_valid) return ""; 
            return validation_error_msg; 
        }
        public object getLastValue() => last_value;
    }


    [Serializable]
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
                if (value != null) {
                    last_value_valid = false;
                    _validate(value);
                    last_value_valid = true;
                    is_null = false;
                }
				this._value = value;
			}
		}
        private TextField() { }
        public TextField(string name = null, string replace_tag = null, string text=null, bool is_readonly = false, string default_value = null, bool is_required = false, string validation_error_msg = "") {
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

    [Serializable]
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
        public BoolField(string name = null, string replace_tag = null, bool value = false, bool read_only = false, bool default_value = false, string validation_error_msg = "") {
            this.name = name; this.replace_tag = replace_tag; this.value = value; this.is_readonly = read_only;
            this.default_value = default_value; this.validation_error_msg = validation_error_msg;
        }

        override public FieldType getType() => FieldType.BOOL;
        override public object getValue()   => this.value;
        override public object getDefault() => this.default_value;
        override public void setValue(object value, bool is_readonly = false) { this.value = (bool)value; this.is_readonly = is_readonly; }
        override public string ToString() => this.value.ToString();
    }


    [Serializable]
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
		public IntergerField(string name = null, string replace_tag = null, long value = 0, bool is_positive = false, bool is_readonly = false, long default_value = 0, bool is_required = false, string validation_error_msg = "") {
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

    [Serializable]
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
		public FloatField(string name = null, string replace_tag = null, double value =0, bool is_positive = false, bool is_readonly = false, double default_value = 0, bool is_required = false, string validation_error_msg = "") {
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

    [Serializable]
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
                is_null = (value == default(DateTime));
            }
		}

        private DateTimeField() { }
		public DateTimeField(string name = null, string replace_tag = null, List<string> replace_tags = null, DateTime datetime = default(DateTime), bool is_readonly = false, DateTime default_value = new DateTime(), Format format = Format.MM_DD_YYYY, bool is_required = false, string validation_error_msg = "") {
            if (replace_tag != null && replace_tags != null) throw new ArgumentException("it's ambigues to decide to use replace_tag and replace_tags");
            if (replace_tags != null) {
                format = Format.DDSUP_MTXT_YYYY;
                setReplaceTags(replace_tags);
            }
            else if (format == Format.DDSUP_MTXT_YYYY) throw new ArgumentException("format DDSUP_MTXT_YYYY can only be use for multiple replace tag");
            // if (replace_tags!= null && format != Format.DDSUP_MTXT_YYYY) throw new ArgumentException(); is_null = false;
			this.name = name; this._value = datetime; this.is_readonly = is_readonly; this.format = format; 
            this.default_value = default_value; this.is_required = is_required; this.validation_error_msg = validation_error_msg;
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

    [Serializable]
    public class EmailField : TextField
    {
        private EmailField() { }
        public EmailField(string name = null, string replace_tag = null, string text = null, bool is_readonly = false, string default_value = null, bool is_required = false, string validation_error_msg = "")
        :base (name: name, replace_tag: replace_tag, text: text, is_readonly : is_readonly, default_value : default_value, is_required : is_required, validation_error_msg: validation_error_msg) { }

        override public FieldType getType() => FieldType.EMAIL;
        public override void _validate(string value) {
            if (!Validator.validateEmail(value)) throw new ValidationError("invalide email");
        }
    }

    [Serializable]
    public class PhoneNumberField : TextField
    {
        private PhoneNumberField() { }
        public PhoneNumberField(string name = null, string replace_tag=null, string text=null, bool is_readonly = false, string default_value = null, bool is_required = false, string validation_error_msg = "")
        : base(name: name, replace_tag: replace_tag, text: text, is_readonly: is_readonly, default_value: default_value, is_required: is_required, validation_error_msg: validation_error_msg) { }

        override public FieldType getType() => FieldType.PHONE_NUMBER;
        public override void _validate(string value) {
            // todo:
        }
    }

    [Serializable]
    public class NICField : TextField
    {
        private NICField() { }
        public NICField(string name = null, string replace_tag = null, string text = null, bool is_readonly = false, string default_value = null, bool is_required = false, string validation_error_msg = "")
        : base(name: name, replace_tag: replace_tag, text: text, is_readonly: is_readonly, default_value: default_value, is_required: is_required, validation_error_msg: validation_error_msg) { }

        override public FieldType getType() => FieldType.NIC;
        public override void _validate(string value) {
            // todo:
        }
    }

    [Serializable]
    public class WebSiteField : TextField
    {
        private WebSiteField() { }
        public WebSiteField(string name = null, string replace_tag = null, string text = null, bool is_readonly = false, string default_value = null, bool is_required = false, string validation_error_msg = "")
        : base(name: name, replace_tag: replace_tag, text: text, is_readonly: is_readonly, default_value: default_value, is_required: is_required, validation_error_msg: validation_error_msg) { }

        override public FieldType getType() => FieldType.WEB_SITE;
        public override void _validate(string value) {
            // todo:
        }
    }

    [Serializable]
    public class DropDownField<T> : Field
    {
        private ObservableCollection<T> items = new ObservableCollection<T>();
        T _value;
        T default_value;
        public T value {
            get { return _value; }
            set {
                if (is_readonly) throw new ReadonlyError();
                if (!items.Contains(value)) {
                    items.Add(value);
                    Logger.logger.logWarning("drop down field add & set item which was not in items list");
                }
                last_value  = value;
                this._value = value;
                is_null     = false;
            }
        }

        private DropDownField() { }
        public DropDownField( string name = null, ObservableCollection<T> items = null, bool is_readonly = false, T default_value = default(T), bool is_required = false, string validation_error_msg = "") {
            this.name = name; this.items = items; this.is_readonly = is_readonly; this.default_value = default_value; this.is_readonly = is_readonly; 
            this.validation_error_msg = validation_error_msg;
        }

        public override object getDefault() => default_value;
        public override FieldType getType() => FieldType.DROP_DOWN;
        public override object getValue() => value;
        public override void setValue(object value, bool is_readonly = false) { 
            if (value != null) { this.value = (T)value; }
            this.is_readonly = is_readonly; 
        }
        public void addItem(T item) { items.Add(item); }
        public void setItemIndex(int index) { this.value = items[index]; } // throws exception if index is out of range
        public int getItemIndex() { return items.IndexOf(value); }
        public void setItems(ObservableCollection<T> items) { 
            this.items = items;
            if (!this.items.Contains(this.value)) { this.value = default(T); is_null = true; }
        }
        public ObservableCollection<T> getItems() => items;
    }
    


}
