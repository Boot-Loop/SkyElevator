using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Core;
using Core.Data.Models;
using CoreApp = Core.Application;

namespace SkyElevator.src.models
{
    public class ProjectModelI : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private ProjectModel _project_model;
        //private ObservableCollection<ClientModel> _client_models;
        private List<string> _project_types = new List<string>();
        private string _button_content = "Next";
        private int _selected_proj_type_index = 0;
        
        public ProjectModel ProjectModel {
            get { return _project_model; }
            set { _project_model = value; }
        }
        public List<ClientModel> ClientModels {
            get { return CoreApp.getSingleton().getClientsDropDownList(); }
            //set { _client_models = value; onPropertyRaised("ClientModels"); }
        }
        public List<string> ProjectTypes
        {
            get { return _project_types; }
            set { _project_types = value; onPropertyRaised("ProjectTypes"); }
        }
        public string ButtonContent {
            get { return _button_content; }
            set { _button_content = value; onPropertyRaised("ButtonContent"); }
        }
        public int SelectedProjectTypeIndex {
            get { return _selected_proj_type_index; }
            set {
                _selected_proj_type_index = value; onPropertyRaised("SelectedProjectTypeIndex");
                _project_model.project_type = ((ProjectManager.ProjectType[])Enum.GetValues(typeof(ProjectManager.ProjectType)))[value];
            }
        }
        public ClientModel SelectedClient {
            get { return _project_model.client.value; }
            set {
                _project_model.client.value = value;
                if (_project_model.client.value == ClientModels[0]) _button_content = "Next";
                else _button_content = "Create";
                _project_model.client.setItemIndex(ClientModels.IndexOf(value));
                onPropertyRaised("SelectedClient");
                onPropertyRaised("ButtonContent");
            }
        }
        public string ProjectName {
            get { return _project_model.name.value; }
            set { _project_model.name.value = value; onPropertyRaised("ProjectName"); }
        }
        public string ProjectLocation {
            get { return _project_model.location.value; }
            set { _project_model.location.value = value; onPropertyRaised("ProjectLocation"); }
        }
        public DateTime ProjectDate {
            get { return _project_model.date.value; }
            set { _project_model.date.value = value; onPropertyRaised("ProjectDate"); }
        }
        public DateTime ProjectCreationDate {
            get { return _project_model.creation_date.value; }
            set { _project_model.creation_date.value = value; onPropertyRaised("ProjectCreationDate"); }
        }
        public ProjectManager.ProjectType ProjectType { 
            get { return _project_model.project_type; }
            set { _project_model.project_type = value; onPropertyRaised("ProjectType"); }
        }

        public ProjectModelI() {
            this.ProjectModel = new ProjectModel();
            //this.ClientModels = ;
            foreach (var type in Enum.GetValues(typeof(ProjectManager.ProjectType))) {
                TextInfo text_info = new CultureInfo("en-US", false).TextInfo;
                _project_types.Add(text_info.ToTitleCase(type.ToString().Replace('_', ' ').ToLower().Replace("or", "/")));
            }
            SelectedClient = ClientModels[0];
            SelectedProjectTypeIndex = 0;
        }

        public override string ToString() {
            return _project_model.name.value;
        }

        private void onPropertyRaised(string property_name) {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(property_name));
        }
    }
}