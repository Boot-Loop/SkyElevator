using Core.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xceed.Words.NET;

namespace Core.Data.Doc
{
    [Serializable]
    public class ElevatorInspectionSheetData : DocumentData
    {

        public TextField        ref_no                      { get; set; } = new TextField(        name:"Ref No",                     replace_tag:"<ref_no>");
        public TextField        contact_person              { get; set; } = new TextField(        name:"Contact Person",             replace_tag:"<contact_person>");
        public TextField        project_name_and_address    { get; set; } = new TextField(        name:"Project Name and Address",   replace_tag:"<project_name_and_address>");
        public TextField        agreement_no                { get; set; } = new TextField(        name:"Agreement No",               replace_tag:"<agreement_no>");
        public TextField        done_by_name                { get; set; } = new TextField(        name:"Done by Name",               replace_tag:"<done_by_name>");
        public TextField        checked_by_name             { get; set; } = new TextField(        name:"Checked by Name",            replace_tag:"<checked_by_name>");
        public DateTimeField    start_date_format2          { get; set; } = new DateTimeField(    name:"Start Date Format2",         replace_tag:"<start_date_format2>",         format : DateTimeField.Format.MM_DD_YYYY);
        public DateTimeField    done_by_date_format2        { get; set; } = new DateTimeField(    name:"Done by Date Format2",       replace_tag:"<done_by_date_format2>",       format : DateTimeField.Format.MM_DD_YYYY);
        public DateTimeField    checked_by_date_format2     { get; set; } = new DateTimeField(    name: "Checked by Date Format2",   replace_tag: "<checked_by_date_format2>",   format: DateTimeField.Format.MM_DD_YYYY );

        override public DocumentType getType() => DocumentType.ELEVATOR_INSPECTION_SHEET;
        override public void setToDefault() { throw new NotImplementedException(); }
    }//ElevatorInspectionSheetData


    public class ElevatorInspectionSheet : Document
    {

        /* fields */
        private ElevatorInspectionSheetData data = new ElevatorInspectionSheetData();

        /* constructor */
        public ElevatorInspectionSheet() { }
        public ElevatorInspectionSheet(string path) : base(path) { }

        /* methods */
        public override DocumentData getData() => data;
        public override DocumentType getType() => DocumentType.ELEVATOR_INSPECTION_SHEET;


        public override void generateDocument(string path)
        {
            if (!Validator.validateFilePath(path, is_new: true) || (path == null)) throw new InvalidPathError();
            var template = DocX.Load(Paths.Template.ELEVATOR_INSPECTION_SHEET);
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
