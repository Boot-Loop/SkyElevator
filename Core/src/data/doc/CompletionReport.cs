using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Utils;
using Xceed.Words.NET;

namespace Core.Data.Doc
{
    [Serializable]
    public class CompletionReportData : IDocumentData
    {
        public List<Field> fields = new List<Field>();

        public TextField        our_ref_no                 = new TextField("Our Ref No", "<our_ref>");
        public DateTimeField    start_date_format1         = new DateTimeField("Start Date Format1", new List<string>() { "<start_day>" , "<start_subscript>", "<start_month_year>" } );
        public DateTimeField    start_date_format2         = new DateTimeField("Start Date Format2", "<start_date_format2>", format: DateTimeField.Format.MM_DD_YYYY );
        public DateTimeField    end_date                   = new DateTimeField("End Date", "<end_date_format2>");
        public TextField        name                       = new TextField("Name", "<name>");
        public TextField        address_no_and_road        = new TextField("Address No and Road", "<address_no_and_road>");
        public TextField        address_city               = new TextField("Address City", "<address_city>");
        public TextField        sir_or_madam               = new TextField("Sir/Madam", "<sir_or_madam>");
        public TextField        agreement_no               = new TextField("Agreemnet No", "<agreement_no>");
        public TextField        recieved_by_name           = new TextField("Recieved by Name", "<recieved_by_name>");

        /*constructor*/
        public CompletionReportData()
        {
            fields.Add(our_ref_no);
            fields.Add(start_date_format1);
            fields.Add(start_date_format2);
            fields.Add(end_date);
            fields.Add(name);
            fields.Add(address_no_and_road);
            fields.Add(address_city);
            fields.Add(sir_or_madam);
            fields.Add(agreement_no);
            fields.Add(recieved_by_name);  
        }

        public DocumentType getType() => DocumentType.COMPLETION_REPORT;

        public void setToDefault() {
            throw new NotImplementedException();
        }
    }//CompletionReportData

    public class CompletionReport : Document
    {

        /* fields */
        private CompletionReportData data = new CompletionReportData();

        /* constructor */
        public CompletionReport() { }
        public CompletionReport(string path) : base(path) { }

        /* methods */
        public override IDocumentData getData() => data;
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
            foreach (Field field in data.fields)
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
