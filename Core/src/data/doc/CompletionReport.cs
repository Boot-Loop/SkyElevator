using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Core.Utils;
using Xceed.Words.NET;

namespace Core.Data.Doc
{
    [Serializable]
    public class CompletionReportData : DocumentData
    {
        public TextField        our_ref_no              { get; set; } = new TextField(     name:"Our Ref No",          replace_tag:"<our_ref>");
        public DateTimeField    start_date_format1      { get; set; } = new DateTimeField( name:"Start Date Format1",  replace_tags: new List<string>() { "<start_day>" , "<start_subscript>", "<start_month_year>" } );
        public DateTimeField    start_date_format2      { get; set; } = new DateTimeField( name:"Start Date Format2",  replace_tag:"<start_date_format2>", format: DateTimeField.Format.MM_DD_YYYY );
        public DateTimeField    end_date                { get; set; } = new DateTimeField( name:"End Date",            replace_tag:"<end_date_format2>");
        public TextField        name                    { get; set; } = new TextField(     name:"Name",                replace_tag:"<name>");
        public TextField        address_no_and_road     { get; set; } = new TextField(     name:"Address No and Road", replace_tag:"<address_no_and_road>");
        public TextField        address_city            { get; set; } = new TextField(     name:"Address City",        replace_tag:"<address_city>");
        public TextField        sir_or_madam            { get; set; } = new TextField(     name:"Sir/Madam",           replace_tag:"<sir_or_madam>");
        public TextField        agreement_no            { get; set; } = new TextField(     name:"Agreemnet No",        replace_tag:"<agreement_no>");
        public TextField        recieved_by_name        { get; set; } = new TextField(     name: "Recieved by Name",   replace_tag: "<recieved_by_name>");

        override public DocumentType getType() => DocumentType.COMPLETION_REPORT;
        override public void setToDefault() { throw new NotImplementedException(); }
    }//CompletionReportData

    public class CompletionReport : Document
    {

        /* fields */
        private CompletionReportData data = new CompletionReportData();

        /* constructor */
        public CompletionReport() { }
        public CompletionReport(string path) : base(path) { }

        /* methods */
        public override DocumentData getData() => data;
        public override DocumentType getType()  => DocumentType.COMPLETION_REPORT;

        public override void loadDocument(bool is_readonly = false) {
            throw new NotImplementedException();
        }

        public override void saveAsDraft() {
            throw new NotImplementedException();
        }

        public override void close() {
            throw new NotImplementedException();
        }

        public override void generateDocument(string path) {
            if (!Validator.validateFilePath(path, is_new: true) || (path == null)) throw new InvalidPathError();
            var template = DocX.Load(Paths.Template.COMPLETION_REPORT);
            foreach (Field field in data._fields)
            {
                if (field.getType() == FieldType.DATE_TIME) { // date time field
                    var datetime_field = (DateTimeField)field;
                    if (!datetime_field.isNull()) {
                        if (datetime_field.getFormat() == DateTimeField.Format.DDSUP_MTXT_YYYY) { 
                            foreach ( var pair in datetime_field.getReplaceTags()) {
                                template.ReplaceText(pair.Key, pair.Value);
                            }
                        }
                        else template.ReplaceText(field.getReplaceTag(), field.ToString());
                    } 
                    else { // date time field is null
                        if (datetime_field.getFormat() == DateTimeField.Format.DDSUP_MTXT_YYYY) {
                            foreach (var pair in datetime_field.getReplaceTags()) {
                                template.ReplaceText(pair.Key, pair.Key);
                            }
                        }
                        else template.ReplaceText(field.getReplaceTag(), "");
                    }
                }
                else { // other fields but date time field
                    if (field.getValue() == null) template.ReplaceText(field.getReplaceTag(), "");
                    else template.ReplaceText(field.getReplaceTag(), field.ToString());                    
                }
            }
            template.SaveAs(path); // TODO: this throws System.IO.IOException if the file already opened!!
        }
    }
}
