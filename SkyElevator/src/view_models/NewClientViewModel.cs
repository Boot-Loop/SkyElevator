using System;
using System.ComponentModel;

using CoreApp = Core.Application;

using static SkyElevator.src.view_models.AlertViewViewModel;
using SkyElevator.src.models;
using SkyElevator.src.view_models.commands;
using SkyElevator.src.views.alert_views;
using SkyElevator.src.views.project_manager_views.sub_views;
using Core.Data;

namespace SkyElevator.src.view_models
{
    public class NewClientViewModel :INotifyPropertyChanged
    {
        private ClientModelI _client_model_i = new ClientModelI();
        private NewClient _new_client;
        private NewClientRelayCommand _create_client_command;
        private NewClientRelayCommand _back_button_command;

        public event PropertyChangedEventHandler PropertyChanged;

        public ClientModelI ClientModelI {
            get { return _client_model_i; }
            set { _client_model_i = value; onPropertyRaised("ClientModelI"); }
        }
        public NewClient NewClient {
            get { return _new_client; }
            set { _new_client = value; }
        }
        public NewClientRelayCommand CreatNewProjectWithClientCommand {
            get { return _create_client_command; }
            set { _create_client_command = value; }
        }
        public NewClientRelayCommand BackButtonCommand {
            get { return _back_button_command; }
            set { _back_button_command = value; }
        }

        public NewClientViewModel(NewClient new_client) {
            this.NewClient = new_client;
            this.CreatNewProjectWithClientCommand   = new NewClientRelayCommand(() => createNewProjectWithClient());
            this.BackButtonCommand                  = new NewClientRelayCommand(() => backButtonCommand());
        }

        public void createNewProjectWithClient() {
            _client_model_i.api.model.id.value = DateTime.Now.Ticks; // TODO: consider change the pk
            _client_model_i.api.update();
            NewProjectViewModel new_project_view_model = NewClient.ProjectManager.NewProject.NewProjectViewModel;
            new_project_view_model.ProjectModelI.SelectedClient = _client_model_i.ClientModel;

            AlertViewViewModel.Button button = new AlertViewViewModel.Button { name = "Okay" };
            try {
                CoreApp.getSingleton().createNewProject(new_project_view_model.ProjectModelI.ProjectModelApi, new_project_view_model.FolderBrowseCommand.FolderPath);
                CoreApp.getSingleton().setDefaultProjectPath(new_project_view_model.FolderBrowseCommand.FolderPath);
                NewClient.ProjectManager.closeWindow();
            }
            catch (Exception err) {
                Core.Reference.logger.logError(err);
                AlertView alertView = new AlertView("Unknown error", "Unknown error has occured while creating a project. Please try again.", AlertViewType.ERROR, button);
                alertView.ShowDialog();
            }
        }
        public void backButtonCommand() {
            NewClient.ProjectManager.ProjectManagerViewModel.backButtonPressed();
        }
        private void onPropertyRaised(string property_name) {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(property_name));
        }
    }
}
