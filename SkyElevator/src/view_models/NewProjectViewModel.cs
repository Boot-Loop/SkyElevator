using SkyElevator.src.models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows;

using Core.Data.Models;
using CoreApp = Core.Application;

using SkyElevator.src.views.home_views;
using SkyElevator.src.view_models.commands;
using SkyElevator.src.views.alert_views;

namespace SkyElevator.src.view_models
{
    public class NewProjectViewModel : INotifyPropertyChanged
    {
        private ProjectModelI _project_model_i = new ProjectModelI();
        private ClientModel _selected_client;
        private FolderBrowseCommand _folderBrowseCommand;
        private string _selected_projet_type;
        private string _button_content = "Next";
        //private string _save_path;


        public event PropertyChangedEventHandler PropertyChanged;

        public ProjectModelI ProjectModelI {
            get { return _project_model_i; }
            set { _project_model_i = value; onPropertyRaised("ProjectModelI"); }
        }
        public ClientModel SelectedClient {
            get { return _selected_client; }
            set { _selected_client = value;
                if (_selected_client == _client_models[0]) _button_content = "Next";
                else _button_content = "Create";
                onPropertyRaised("SelectedClient");
                onPropertyRaised("ButtonContent");
                _project_model_i.ProjectModel.client.setItemIndex( _client_models.IndexOf(value) );
            }
        }
        public FolderBrowseCommand FolderBrowseCommand {
            get { return _folderBrowseCommand; }
            set { _folderBrowseCommand = value; onPropertyRaised("FolderBrowseCommand"); }
        }
        public string SelectedProjectType {
            get { return _selected_projet_type; }
            set { _selected_projet_type = value; onPropertyRaised("SelectedProjectType"); }
        }
        public string ButtonContent {
            get { return _button_content; }
            set { _button_content = value; onPropertyRaised("ButtonContent"); }
        }

        private ObservableCollection<ClientModel> _client_models = CoreApp.getSingleton().getClientsDropDownList();
        private List<string> _project_types = new List<string> { "Installation", "Maintenance", "Repair/Modernization", "Others" };
        
        public ObservableCollection<ClientModel> ClientModels {
            get { return _client_models; }
            set { _client_models = value; onPropertyRaised("ClientModels"); }
        }
        public List<string> ProjectTypes {
            get { return _project_types; }
            set { _project_types = value; onPropertyRaised("ProjectTypes"); }
        }

        public NewProjectViewModel() {
            SelectedClient = _client_models[0];
            SelectedProjectType = _project_types[0];
            FolderBrowseCommand = new FolderBrowseCommand();
        }

        public void nextOrCreateCommand() {
            if (string.IsNullOrWhiteSpace(_project_model_i.ProjectName) || string.IsNullOrWhiteSpace(_project_model_i.ProjectLocation) || _project_model_i.ProjectModel == null || _project_model_i.ProjectDate == DateTime.MinValue) {
                //TODO: Implement Message box showing empty fields to be filled.
                AlertView alertView = new AlertView();
                alertView.ShowDialog();
            }
            else {
                DateTime current_date_time = DateTime.Now;
                _project_model_i.ProjectCreationDate = current_date_time;
                if (_selected_client == _client_models[0]) {

                }
                else {
                    CoreApp.getSingleton().createNewProject(FolderBrowseCommand.FolderPath, _project_model_i.ProjectModel);
                }
            }
        }

        private void onPropertyRaised(string property_name) {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(property_name));
        }
    }
}
