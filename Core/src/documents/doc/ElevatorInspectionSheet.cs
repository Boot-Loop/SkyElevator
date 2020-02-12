using Core.utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xceed.Words.NET;

namespace Core.src.documents
{
    public class ElevatorInspectionSheetData : IDocumentData
    {
        public List<IField> fields = new List<IField>();

        public TextField        ref_no                      = new TextField("Ref No", "<ref_no>");
        public TextField        contact_person              = new TextField("Contact Person", "<contact_person>");
        public TextField        project_name_and_address    = new TextField("Project Name and Address", "<project_name_and_address>");
        public TextField        agreement_no                = new TextField("Agreement No", "<agreement_no>");
        public TextField        done_by_name                = new TextField("Done by Name", "<done_by_name>");
        public TextField        checked_by_name             = new TextField("Checked by Name", "<checked_by_name>");
        public DateTimeField    start_date_format2          = new DateTimeField("Start Date Format2", "<start_date_format2>", format : DateTimeField.Format.MM_DD_YYYY);
        public DateTimeField    done_by_date_format2        = new DateTimeField("Done by Date Format2", "<done_by_date_format2>", format : DateTimeField.Format.MM_DD_YYYY);
        public DateTimeField    checked_by_date_format2     = new DateTimeField("Checked by Date Format2", "<checked_by_date_format2>", format : DateTimeField.Format.MM_DD_YYYY);

        /*constructor*/
        public ElevatorInspectionSheetData()
        {
            fields.Add(ref_no                    );
            fields.Add(contact_person            );
            fields.Add(project_name_and_address  );
            fields.Add(agreement_no              );
            fields.Add(done_by_name              );
            fields.Add(checked_by_name           );
            fields.Add(start_date_format2        );
            fields.Add(done_by_date_format2      );
            fields.Add(checked_by_date_format2   );
        }

        public DocumentType getType() => DocumentType.ELEVATOR_INSPECTION_SHEET;

        public void setToDefault() {
            throw new NotImplementedException();
        }
    }//ElevatorInspectionSheetData


    public class ElevatorInspectionSheet : Document
    {

        /* fields */
        private ElevatorInspectionSheetData data = new ElevatorInspectionSheetData();

        /* constructor */
        public ElevatorInspectionSheet() { }
        public ElevatorInspectionSheet(string path) : base(path) { }

        /* methods */
        public override IDocumentData getData() => data;
        public override DocumentType getType() => DocumentType.ELEVATOR_INSPECTION_SHEET;


        public override void generateDocument(string path)
        {
            if (!Validator.validateFilePath(path, is_new: true) || (path == null)) throw new InvalidPathError();
            var template = DocX.Load(Paths.Template.ELEVATOR_INSPECTION_SHEET);
            foreach (IField field in data.fields) {
                if (field.getType() == FieldType.DATE_TIME)
                { // date time field
                    var datetime_field = (DateTimeField)field;
                    if (!datetime_field.isNull()) template.ReplaceText(field.getReplaceTag(), field.ToString());

                    else template.ReplaceText(field.getReplaceTag(), "");
                }
                else
                { // other fields but date time field
                    if (field.getValue() == null) template.ReplaceText(field.getReplaceTag(), "");
                    else template.ReplaceText(field.getReplaceTag(), field.ToString());
                }
            }
            template.SaveAs(path); 
        }

        public override void close() {
            throw new NotImplementedException();
        }

        public override void loadDocument(bool is_readonly = false) {
            throw new NotImplementedException();
        }

        public override void saveAsDraft() {
            throw new NotImplementedException();
        }
    }
}
