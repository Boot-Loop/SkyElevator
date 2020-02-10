using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.src;
using Xceed.Words.NET;

namespace Core.src.documents
{
    public class CompletionReportData : IDocumentData
    {
        public List<IField> fields = new List<IField>();

        public DateTimeField    start_date_format1         = new DateTimeField("Start Date Format1", "<start_date_format1>", format : DateTimeField.Format.DDSUP_MTXT_YYYY); // format1 ex: 5th February 2020
        public DateTimeField    start_date_format2         = new DateTimeField("Start Date Format2", "<start_date_format2>", format: DateTimeField.Format.MM_DD_YYYY ); // format2 ex: 05/02/2020
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

        public override void loadDocument() {
            throw new NotImplementedException();
        }

        public override void saveAsDraft() {
            throw new NotImplementedException();
        }

        public override void close() {
            throw new NotImplementedException();
        }

        public override void checkDocumentType() {
            var condition = false;
            if (condition)
            {
                throw new InvalidFileTypeError("Opened document was not a completion report.");
            }
        }

        public override void generateDocument()
        {
            var template = DocX.Load(Paths.Template.COMPLETION_REPORT);
            foreach (IField field in data.fields)
            {
                if (field.getValue() == null) template.ReplaceText(field.getReplaceTag(), field.getReplaceTag());
                else template.ReplaceText(field.getReplaceTag(), field.ToString());
            }
            if (path == null) throw new InvalidPathError();

            template.SaveAs(path); // TODO: this throws System.IO.IOException if the file already opened!!
        }
    }
}
