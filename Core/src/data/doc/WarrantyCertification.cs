using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Utils;
using Xceed.Words.NET;

namespace Core.Data.Doc
{
    public class WarrantyCertificationData : IDocumentData
        {
            public List<Field> fields = new List<Field>();

            public TextField        ref_no                  = new TextField("Ref No", "<ref_no>");
            public DateTimeField    start_date_format3      = new DateTimeField("Start Date Format3", "<start_date_format3>", format: DateTimeField.Format.MTXT_D_YYYY);
            public DateTimeField    start_date_format2      = new DateTimeField("Start Date Format2", "<start_date_format2>", format: DateTimeField.Format.MM_DD_YYYY);
            public TextField        project_name            = new TextField("Project Name", "<project_name>");
            public TextField        contact_person          = new TextField("Contact Person", "<contact_person>");
            public TextField        location                = new TextField("Location", "<location>");
            public TextField        agreement_no            = new TextField("Agreement No", "<agreement_no>");
            public TextField        supplier_name           = new TextField("Supplier Name", "<supplier_name>");

            /*constructor*/
            public WarrantyCertificationData()
            {
                fields.Add(ref_no               ); 
                fields.Add(start_date_format3   );
                fields.Add(start_date_format2   );
                fields.Add(project_name         );
                fields.Add(contact_person       );
                fields.Add(location             );
                fields.Add(agreement_no         );
                fields.Add(supplier_name        );
            }                             

            public DocumentType getType() => DocumentType.WARRENTY_CERTIFICATION;

            public void setToDefault() {
                throw new NotImplementedException();
            }
        }//Warranty certification data

    public class WarrantyCertification : Document
    {

        /* fields */
        private WarrantyCertificationData data = new WarrantyCertificationData();

        /* constructor */
        public WarrantyCertification() { }
        public WarrantyCertification(string path) : base(path) { }

        /* methods */
        public override IDocumentData getData() => data;
        public override DocumentType getType() => DocumentType.WARRENTY_CERTIFICATION;

        public override void generateDocument(string path) {
            if (!Validator.validateFilePath(path, is_new: true) || (path == null)) throw new InvalidPathError();
            var template = DocX.Load(Paths.Template.WARRANTY_CERTIFICATION);
            foreach (Field field in data.fields) {
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

