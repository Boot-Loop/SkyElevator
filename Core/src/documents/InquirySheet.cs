﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xceed.Words.NET;
using System.IO;

using Core.src;

namespace Core.src.documents
{
	public class InquirySheetData : IDocumentData
	{
        public List<IField> fields = new List<IField>();

        public TextField  type                     =  new TextField("Type"                   , "<type>");
        public TextField  capacity                 =  new TextField("Capacity"               , "<capacity>" );
        public TextField  speed                    =  new TextField("Speed"                  , "<speed>" );
        public TextField  travel_height            =  new TextField("Travel Height"          , "<travel height>" );
        public TextField  serving_floors_stops     =  new TextField("Serving Floors Stops"   , "<serving floors>" );
        public TextField  floor_displaying         =  new TextField("Floor Displaying"       , "<floor displaying>" );
        public TextField  quantity                 =  new TextField("Quantity"               , "<quantity>" );
        public TextField  motion_controller        =  new TextField("Motion Controller"      , "<motion controller>" );
        public TextField  drive_control            =  new TextField("Drive Control"          , "<drive control>" );
        public TextField  position_mc_room         =  new TextField("Position M/C room"      , "<position of m/c room>" );
        public TextField  door_operator            =  new TextField("Door Operator"          , "<door operator>" );
        public TextField  door_open_type           =  new TextField("Dor Open Type"          , "<door open type>" );
        public TextField  power_supply             =  new TextField("Power Supply"           , "<power supply>" );
        public TextField  car_dimension            =  new TextField("Car Dimension"          , "<car dimension>" );
        public TextField  car_side_walls           =  new TextField("Car Side Walls"         , "<car side walls>" );
        public TextField  rear_walls               =  new TextField("Rear Walls"             , "<rear walls>" );
        public TextField  car_door                 =  new TextField("Car Door"               , "<car doors>" );
        public TextField  hand_rail                =  new TextField("Hand Rail"              , "<hand rail>" );
        public TextField  flooring                 =  new TextField("Flooring"               , "<flooring>" );
        public TextField  ceiling_light            =  new TextField("Ceiling Light"          , "<ceiling light>" );
        public TextField  ventilation              =  new TextField("Ventilation"            , "<ventilation>" );
        public TextField  car_sill                 =  new TextField("Car Sill"               , "<car sill>" );
        public TextField  safety                   =  new TextField("Safety"                 , "<safety>" );
        public TextField  pit_buffers              =  new TextField("Pit Buffers"            , "<pit buffers>" );
        public TextField  available                =  new TextField("Available"              , "<available>" );
        public TextField  overhead_height          =  new TextField("Overhead Height"        , "<overhead height>" );
        public TextField  pit_depth                =  new TextField("Pit Depth"              , "<pit depth>" );
        public TextField  indicator_type           =  new TextField("Indicator Type"         , "<indicator type>" );
        public TextField  push_button              =  new TextField("Push Button"            , "<push button>" );
        public TextField  landing_door_size        =  new TextField("Landing Door Size"      , "<landing door size>" );
        public TextField  landing_door_finishing   =  new TextField("Landing Door Finishing" , "<landing door finishing>" );
        public TextField  landing_door_jambs       =  new TextField("Landing Door Jambs"     , "<landing door jambs>");

        /* constructor */
        public InquirySheetData()
        {
            fields.Add(type);
            fields.Add(capacity);
            fields.Add(speed);
            fields.Add(travel_height);
            fields.Add(serving_floors_stops);
            fields.Add(floor_displaying);
            fields.Add(quantity);
            fields.Add(motion_controller);
            fields.Add(drive_control);
            fields.Add(position_mc_room);
            fields.Add(door_operator);
            fields.Add(door_open_type);
            fields.Add(power_supply);
            fields.Add(car_dimension);
            fields.Add(car_side_walls);
            fields.Add(rear_walls);
            fields.Add(car_door);
            fields.Add(hand_rail);
            fields.Add(flooring);
            fields.Add(ceiling_light);
            fields.Add(ventilation);
            fields.Add(car_sill);
            fields.Add(safety);
            fields.Add(pit_buffers);
            fields.Add(available);
            fields.Add(overhead_height);
            fields.Add(pit_depth);
            fields.Add(indicator_type);
            fields.Add(push_button);
            fields.Add(landing_door_size);
            fields.Add(landing_door_finishing);
            fields.Add(landing_door_jambs);
        }
        
        /*
		public void copyFrom(IDocumentData data) { // this method might be unnecessary!!
			if (data.getType() != DocumentType.INQUERY_SHEET) throw new ArgumentException();
			InquirySheetData idata = (InquirySheetData)data;
			this.speed.value	= idata.speed.value;
			this.capacity.value = idata.capacity.value;
			this.travel_height.value = idata.capacity.value;
			// TODO: copy every thing from idata to this 
		}
        */

		public void setToDefault() {

        }

		public DocumentType getType() => DocumentType.INQUERY_SHEET;

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
		override public IDocumentData getData() => data;

        override public void loadDocument() {

            if (path == null) throw new InvalidFilePathError();

            var document = DocX.Load(path);
            ///Check whether the loaded document is an inquiry sheet or not
            checkDocumentType();

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

            for (int i = 2; i < document_data.Count; i++){
                if (i % 2 == 0){
                    data.fields[i / 2 - 1 ].setValue(document_data[i]); /* TODO: all values are assumed as strings <- might be an error in future! */
                }
            }

        }

		override public void close() {
			throw new NotImplementedException();
		}

        public override void checkDocumentType()
        {
            var condition = false;
            if (condition) {
                throw new InvalidFileError("Opened document was not an Inquiry sheet.");
            }
        }

        override public void saveAsDraft()
        {
            var template = DocX.Load(Paths.Template.INQUERY_SHEET);
            foreach ( IField field in data.fields) {
                if (field.getValue() == null)  template.ReplaceText(field.getReplaceTag(), field.getReplaceTag());
                else template.ReplaceText(field.getReplaceTag(), field.getValue().ToString());
            }
            if (path == null) throw new InvalidFilePathError();
            
            template.SaveAs(path); // TODO: this throws System.IO.IOException if the file already opened!!
        }
    }
}