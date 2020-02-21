using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Core;
using Core.Data;
using Core.Data.Models;
using CoreApp = Core.Application;

namespace SkyElevator.src.models
{
    public class ProjectModelI : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private static readonly int NULL_CLIENT_PK = 0;

        private ModelAPI<ProjectModel> api = new ModelAPI<ProjectModel>(null, ModelApiMode.MODE_CREATE);
        private List<string> _project_types = new List<string>();
        private string _button_content = "Next";
        private int _selected_proj_type_index = 0;
        
        public ModelAPI<ProjectModel> ProjectModelApi { get { return api; } }
        public ProjectModel ProjectModel {
            get { return api.model; }
            set { api.model = value; }
        }
        private List<ClientModel> _client_models = CoreApp.getSingleton().getClientsDropDownList();
        public List<ClientModel> ClientModels {
            get { return _client_models; }
            set { _client_models = value; onPropertyRaised("ClientModels"); }
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
                api.model.setProjectType(((ProjectManager.ProjectType[])Enum.GetValues(typeof(ProjectManager.ProjectType)))[value]);
                // _project_model.project_type.value = value;
            }
        }
        ClientModel _selected_client;
        public ClientModel SelectedClient {
            get {
                return _selected_client;
            } 
            set {
                _selected_client = value;
                if (_selected_client.id.value == ClientModels[NULL_CLIENT_PK].id.value) _button_content = "Next";
                else _button_content = "Create";
                api.model.client_id.value = value.id.value;
                onPropertyRaised("SelectedClient");
                onPropertyRaised("ButtonContent");
            }
        }
        public string ProjectName {
            get { return api.model.name.value; }
            set { api.model.name.value = value; onPropertyRaised("ProjectName"); }
        }
        public string ProjectLocation {
            get { return api.model.location.value; }
            set { api.model.location.value = value; onPropertyRaised("ProjectLocation"); }
        }
        public DateTime ProjectDate {
            get { return api.model.date.value; }
            set { api.model.date.value = value; onPropertyRaised("ProjectDate"); }
        }
        public DateTime ProjectCreationDate {
            get { return api.model.creation_date.value; }
            set { api.model.creation_date.value = value; onPropertyRaised("ProjectCreationDate"); }
        }
        public ProjectManager.ProjectType ProjectType { 
            get { return api.model.getProjectType(); }
            set { api.model.setProjectType(value); onPropertyRaised("ProjectType"); }
        }

        public ProjectModelI() {
            foreach (var type in Enum.GetValues(typeof(ProjectManager.ProjectType))) {
                TextInfo text_info = new CultureInfo("en-US", false).TextInfo;
                _project_types.Add(text_info.ToTitleCase(type.ToString().Replace('_', ' ').ToLower().Replace("or", "/")));
            }
            SelectedClient = ClientModels[NULL_CLIENT_PK];
            SelectedProjectTypeIndex = 0;
        }

        public override string ToString() {
            return api.model.name.value;
        }

        private void onPropertyRaised(string property_name) {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(property_name));
        }
    }
}