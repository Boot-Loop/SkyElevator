using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Core.Data.Models;

namespace SkyElevator.src.models
{
    public class ProjectModelI : INotifyPropertyChanged
    {
        private ProjectModel _project_model = new ProjectModel();

        public event PropertyChangedEventHandler PropertyChanged;

        public ProjectModel ProjectModel {
            get { return _project_model; }
        }

        public ProjectModelI() {
            //_project_model.name.value = "asf";
            //_project_model.location.value = "ssss";
            //_project_model.date.value = DateTime.Now;
        }

        public string ProjectName {
            get { return _project_model.name.value; }
            set { _project_model.name.value = value; onPropertyRaised("ProjectName"); }
        }
        public string ProjectLocation {
            get { return _project_model.location.value; }
            set { _project_model.location.value = value; onPropertyRaised("ProjectLocation"); }
        }
        public ClientModel Client {
            get { return _project_model.client.value; }
            set { _project_model.client.value = value; onPropertyRaised("Client"); }
        }
        public DateTime ProjectDate {
            get { return _project_model.date.value; }
            set { _project_model.date.value = value; onPropertyRaised("ProjectDate"); }
        }
        public DateTime ProjectCreationDate {
            get { return _project_model.creation_date.value; }
            set { _project_model.creation_date.value = value; onPropertyRaised("ProjectCreationDate"); }
        }

        public override string ToString()
        {
            return _project_model.name.value;
        }

        private void onPropertyRaised(string property_name) {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(property_name));
        }
    }
}
