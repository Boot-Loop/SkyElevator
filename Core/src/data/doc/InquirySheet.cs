using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xceed.Words.NET;
using System.IO;

using Core.Utils;
using System.Reflection;

namespace Core.Data.Doc
{
    [Serializable]
    public class InquirySheetData : DocumentData
	{
        public TextField  client_name              { get; set; } =  new TextField( name:"Client Name"            , replace_tag:"<client name>");
        public TextField  type                     { get; set; } =  new TextField( name:"Type"                   , replace_tag:"<type>");
        public TextField  capacity                 { get; set; } =  new TextField( name:"Capacity"               , replace_tag:"<capacity>" );
        public TextField  speed                    { get; set; } =  new TextField( name:"Speed"                  , replace_tag:"<speed>" );
        public TextField  travel_height            { get; set; } =  new TextField( name:"Travel Height"          , replace_tag:"<travel height>" );
        public TextField  serving_floors_stops     { get; set; } =  new TextField( name:"Serving Floors Stops"   , replace_tag:"<serving floors>" );
        public TextField  floor_displaying         { get; set; } =  new TextField( name:"Floor Displaying"       , replace_tag:"<floor displaying>" );
        public TextField  quantity                 { get; set; } =  new TextField( name:"Quantity"               , replace_tag:"<quantity>" );
        public TextField  motion_controller        { get; set; } =  new TextField( name:"Motion Controller"      , replace_tag:"<motion controller>" );
        public TextField  drive_control            { get; set; } =  new TextField( name:"Drive Control"          , replace_tag:"<drive control>" );
        public TextField  position_mc_room         { get; set; } =  new TextField( name:"Position M/C room"      , replace_tag:"<position of m/c room>" );
        public TextField  door_operator            { get; set; } =  new TextField( name:"Door Operator"          , replace_tag:"<door operator>" );
        public TextField  door_open_type           { get; set; } =  new TextField( name:"Dor Open Type"          , replace_tag:"<door open type>" );
        public TextField  power_supply             { get; set; } =  new TextField( name:"Power Supply"           , replace_tag:"<power supply>" );
        public TextField  car_dimension            { get; set; } =  new TextField( name:"Car Dimension"          , replace_tag:"<car dimension>" );
        public TextField  car_side_walls           { get; set; } =  new TextField( name:"Car Side Walls"         , replace_tag:"<car side walls>" );
        public TextField  rear_walls               { get; set; } =  new TextField( name:"Rear Walls"             , replace_tag:"<rear walls>" );
        public TextField  car_door                 { get; set; } =  new TextField( name:"Car Door"               , replace_tag:"<car doors>" );
        public TextField  hand_rail                { get; set; } =  new TextField( name:"Hand Rail"              , replace_tag:"<hand rail>" );
        public TextField  flooring                 { get; set; } =  new TextField( name:"Flooring"               , replace_tag:"<flooring>" );
        public TextField  ceiling_light            { get; set; } =  new TextField( name:"Ceiling Light"          , replace_tag:"<ceiling light>" );
        public TextField  ventilation              { get; set; } =  new TextField( name:"Ventilation"            , replace_tag:"<ventilation>" );
        public TextField  car_sill                 { get; set; } =  new TextField( name:"Car Sill"               , replace_tag:"<car sill>" );
        public TextField  safety                   { get; set; } =  new TextField( name:"Safety"                 , replace_tag:"<safety>" );
        public TextField  pit_buffers              { get; set; } =  new TextField( name:"Pit Buffers"            , replace_tag:"<pit buffers>" );
        public TextField  available                { get; set; } =  new TextField( name:"Available"              , replace_tag:"<available>" );
        public TextField  overhead_height          { get; set; } =  new TextField( name:"Overhead Height"        , replace_tag:"<overhead height>" );
        public TextField  pit_depth                { get; set; } =  new TextField( name:"Pit Depth"              , replace_tag:"<pit depth>" );
        public TextField  indicator_type           { get; set; } =  new TextField( name:"Indicator Type"         , replace_tag:"<indicator type>" );
        public TextField  push_button              { get; set; } =  new TextField( name:"Push Button"            , replace_tag:"<push button>" );
        public TextField  landing_door_size        { get; set; } =  new TextField( name:"Landing Door Size"      , replace_tag:"<landing door size>" );
        public TextField  landing_door_finishing   { get; set; } =  new TextField( name:"Landing Door Finishing" , replace_tag:"<landing door finishing>" );
        public TextField landing_door_jambs        { get; set; } =  new TextField( name: "Landing Door Jambs"    , replace_tag: "<landing door jambs>");

		override public void setToDefault() { throw new NotImplementedException(); }
		override public DocumentType getType() => DocumentType.INQUERY_SHEET;

	} // InquirySheetData


	/******* IMPLIMENTATION OF INQUIRY SHEET ********/
	public class InquirySheet : Document
	{
		/* fields */
		private InquirySheetData data = new InquirySheetData();

        /* constructor */
        public InquirySheet() { }
        public InquirySheet( string path ) : base(path) { }

		/* methods */
		override public DocumentType getType()  => DocumentType.INQUERY_SHEET;
		override public DocumentData getData() => data;

        override public void loadDocument(bool is_readonly = false) {

            if (path == null) throw new InvalidPathError();

            var document = DocX.Load(path);
            Document.checkDocument(path, getType()); // Check whether the loaded document is an inquiry sheet or not   

            List<String> document_data = new List<string>();
            for (int i = 0; i < document.Paragraphs.Count; i++){
                foreach (var item in document.Paragraphs[i].Text.Split(new string[] { "\n" }
                          , StringSplitOptions.RemoveEmptyEntries)){
                    document_data.Add(item);
                }
            }
            /*Removing header rows and subtitle rows*/ 
            /* TODO: define them as constant and remove the megic numbers */
            document_data.RemoveRange(0, 7);
            document_data.RemoveAt(26);
            document_data.RemoveAt(48);
            document_data.RemoveAt(54);
            document_data.RemoveAt(58);

            for (int i = 2; i < document_data.Count; i++) {
                if (i % 2 == 0){
                    data._fields[i / 2].setValue(document_data[i], is_readonly); /* TODO: all values are assumed as strings <- might be an error in future! */
                }
            }

        }

		override public void close() {
			throw new NotImplementedException();
		}


        override public void saveAsDraft() {
            var template = DocX.Load(Paths.Template.INQUERY_SHEET);
            foreach ( Field field in data._fields) {
                if (field.getValue() == null)  template.ReplaceText(field.getReplaceTag(), field.getReplaceTag());
                else template.ReplaceText(field.getReplaceTag(), field.getValue().ToString());
            }
            if (path == null) throw new InvalidPathError();
            
            template.SaveAs(path); // TODO: this throws System.IO.IOException if the file already opened!!
        }

        public override void generateDocument(string path) {
            if (!Validator.validateFilePath(path, is_new: true) || (path == null)) throw new InvalidPathError();
            var template = DocX.Load(Paths.Template.INQUERY_SHEET);
            foreach (Field field in data._fields) {
                if (field.getValue() == null)  template.ReplaceText(field.getReplaceTag(), "");
                else template.ReplaceText(field.getReplaceTag(), field.getValue().ToString());
            }
            template.SaveAs(path);
        }
    }
}
