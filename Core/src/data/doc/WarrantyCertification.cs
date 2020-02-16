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
    public class WarrantyCertificationData : DocumentData
        {
            public TextField        ref_no                  { get; set; } = new TextField(        name:"Ref No",              replace_tag:"<ref_no>");
            public DateTimeField    start_date_format3      { get; set; } = new DateTimeField(    name:"Start Date Format3",  replace_tag:"<start_date_format3>", format: DateTimeField.Format.MTXT_D_YYYY);
            public DateTimeField    start_date_format2      { get; set; } = new DateTimeField(    name:"Start Date Format2",  replace_tag:"<start_date_format2>", format: DateTimeField.Format.MM_DD_YYYY);
            public TextField        project_name            { get; set; } = new TextField(        name:"Project Name",        replace_tag:"<project_name>");
            public TextField        contact_person          { get; set; } = new TextField(        name:"Contact Person",      replace_tag:"<contact_person>");
            public TextField        location                { get; set; } = new TextField(        name:"Location",            replace_tag:"<location>");
            public TextField        agreement_no            { get; set; } = new TextField(        name:"Agreement No",        replace_tag:"<agreement_no>");
            public TextField        supplier_name           { get; set; } = new TextField(        name: "Supplier Name",      replace_tag: "<supplier_name>");                      

            override public DocumentType getType() => DocumentType.WARRENTY_CERTIFICATION;
            override public void setToDefault() { throw new NotImplementedException(); }

        }//Warranty certification data

    public class WarrantyCertification : Document
    {

        /* fields */
        private WarrantyCertificationData data = new WarrantyCertificationData();

        /* constructor */
        public WarrantyCertification() { }
        public WarrantyCertification(string path) : base(path) { }

        /* methods */
        public override DocumentData getData() => data;
        public override DocumentType getType() => DocumentType.WARRENTY_CERTIFICATION;

        public override void generateDocument(string path) {
            if (!Validator.validateFilePath(path, is_new: true) || (path == null)) throw new InvalidPathError();
            var template = DocX.Load(Paths.Template.WARRANTY_CERTIFICATION);
            foreach (Field field in data._fields) {
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

