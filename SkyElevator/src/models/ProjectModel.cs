using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Core.Data;

namespace SkyElevator.src.models
{
    public class ProjectModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private TextField _project_name                 = new TextField();
        private TextField _project_location             = new TextField();
        private ClientModel _client                     = new ClientModel();
        private DateTimeField _project_date             = new DateTimeField();
        private DateTimeField _project_creation_date    = new DateTimeField();
        private string _save_path                       = "";

        public ProjectModel() {

        }

        public string ProjectName {
            get { return _project_name.value; }
            set { }
        }
        public string ProjectLocation {
            get { return _project_location.value; }
            set { }
        }
        public ClientModel Client {
            get { return _client; }
            set { }
        }
        public string ProjectDate {
            get { return _project_date.value.ToString(); }
            set { }
        }
        public DateTimeField ProjectCreationDate {
            get { return _project_creation_date; }
            set { }
        }
        public string SavePath {
            get { return _save_path; }
            set { }
        }
        private void onPropertyRaised(string property_name) {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(property_name));
        }
    }
}
