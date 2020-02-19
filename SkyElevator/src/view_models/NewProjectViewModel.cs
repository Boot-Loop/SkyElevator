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
using Ap = Core.Application;

using SkyElevator.src.views.home_views;

namespace SkyElevator.src.view_models
{
    public class NewProjectViewModel : INotifyPropertyChanged
    {
        private ProjectModelI _project_model_i = new ProjectModelI();
        private ClientModel _selected_client;
        private string _button_content = "Next";
        private string _save_path;


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
        public string ButtonContent {
            get { return _button_content; }
            set { _button_content = value; onPropertyRaised("ButtonContent"); }
        }
        public string SavePath {
            get { return _save_path; }
            set { _save_path = value; onPropertyRaised("SavePath"); }
        }

        private ObservableCollection<ClientModel> _client_models = Ap.getSingleton().getClientsDropDownList();
        
        public ObservableCollection<ClientModel> ClientModels {
            get { return _client_models; }
            set { _client_models = value; onPropertyRaised("ClientModels"); }
        }

        public NewProjectViewModel() {
            _selected_client = _client_models[0];
        }

        public void nextOrCreateCommand() {
            if (_selected_client != _client_models[0]) {
                DateTime current_date_time = DateTime.Now;
                _project_model_i.ProjectCreationDate = current_date_time;
                Ap.getSingleton().createNewProject(_save_path, _project_model_i.ProjectModel);
                Home home = new Home();
                home.Show();
            }
            else Console.WriteLine("Not selected");
        }

        public void folderBrowserDialogCommand() {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            DialogResult dialogResult = folderBrowserDialog.ShowDialog();
            if (dialogResult == DialogResult.OK) _save_path = folderBrowserDialog.SelectedPath;
        }

        private void onPropertyRaised(string property_name) {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(property_name));
        }
    }
}
