using System;
using System.ComponentModel;
using System.IO;
using SystemApp = System.Windows.Application;

using CoreApp = Core.Application;
using Core;

using SkyElevator.src.models;
using SkyElevator.src.view_models.commands;
using SkyElevator.src.views.alert_views;
using static SkyElevator.src.view_models.AlertViewViewModel;
using SkyElevator.src.views.project_manager_views.sub_views;
using Core.Utils;
using Core.Data.Models;

namespace SkyElevator.src.view_models
{
    public class NewProjectViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private ProjectModelI _project_model_i;
        private FolderBrowseCommand _folderBrowseCommand;
        private NewProject _new_project;
        private NewProjectRelayCommand _next_or_create_command;


        public ProjectModelI ProjectModelI {
            get { return _project_model_i; }
            set { _project_model_i = value; onPropertyRaised("ProjectModelI"); }
        }
        public FolderBrowseCommand FolderBrowseCommand {
            get { return _folderBrowseCommand; }
            set { _folderBrowseCommand = value; onPropertyRaised("FolderBrowseCommand"); }
        }
        public NewProject NewProject {
            get { return _new_project; }
            set { _new_project = value; }
        }
        public NewProjectRelayCommand NextOrCreateCommand {
            get { return _next_or_create_command; }
            set { _next_or_create_command = value; }
        }

        public NewProjectViewModel(NewProject new_project) {
            this.ProjectModelI = new ProjectModelI();
            this.FolderBrowseCommand = new FolderBrowseCommand();
            this.NewProject = new_project;
            this.NextOrCreateCommand = new NewProjectRelayCommand(() => nextOrCreateCommand());
        }

        public void nextOrCreateCommand() {
            AlertViewViewModel.Button button = new AlertViewViewModel.Button { name = "Okay" };
            if (string.IsNullOrWhiteSpace(_project_model_i.ProjectName) || string.IsNullOrWhiteSpace(_project_model_i.ProjectLocation) || _project_model_i.ProjectModel == null || _project_model_i.ProjectDate == DateTime.MinValue) {
                AlertView alertView = new AlertView("Not enough informations", "The informations you have provided to create a new project is not sufficient. Please fill essential fields.", AlertViewType.WARNING, button);
                alertView.ShowDialog();
            }
            else {
                DateTime current_date_time = DateTime.Now;
                _project_model_i.ProjectCreationDate = current_date_time;
                if (_project_model_i.ProjectModel.client.value == _project_model_i.ClientModels[0]) {
                    try {
                        this.nextButtonClicked();
                    }
                    catch (AlreadyExistsError) {
                        AlertView alertView = new AlertView("Project already exist", "The project you are trying to create is already exist, try a different project name.", AlertViewType.ERROR, button);
                        alertView.ShowDialog();
                    }
                    catch (DirectoryNotFoundException) {
                        AlertView alertView = new AlertView("Invalid path", "The path you are trying to create the project is invalid, insert a correct path or explore a path.", AlertViewType.ERROR, button);
                        alertView.ShowDialog();
                    }
                    catch (Exception err) {
                        Core.Reference.logger.logError(err);
                        AlertView alertView = new AlertView("Unknown error", "Unknown error has occured while creating a project. Please try again.", AlertViewType.ERROR, button);
                        alertView.ShowDialog();
                    }
                }
                else {
                    try {
                        CoreApp.getSingleton().createNewProject(ProjectModelI.ProjectModel, FolderBrowseCommand.FolderPath);
                        CoreApp.getSingleton().setDefaultProjectPath(FolderBrowseCommand.FolderPath);
                        NewProject.ProjectManager.closeWindow();
                    }
                    catch (AlreadyExistsError) {
                        AlertView alertView = new AlertView("Project already exist", "The project you are trying to create is already exist, try a different project name.", AlertViewType.ERROR, button);
                        alertView.ShowDialog();
                    }
                    catch (DirectoryNotFoundException) {
                        AlertView alertView = new AlertView("Invalid path", "The path you are trying to create the project is invalid, insert a correct path or explore a path.", AlertViewType.ERROR, button);
                        alertView.ShowDialog();
                    }
                    catch (Exception err) {
                        Core.Reference.logger.logError(err);
                        AlertView alertView = new AlertView("Unknown error", "Unknown error has occured while creating a project. Please try again.", AlertViewType.ERROR, button);
                        alertView.ShowDialog();
                    }
                }
            }
        }

        private void nextButtonClicked() {
            if (!Directory.Exists(FolderBrowseCommand.FolderPath)) throw new DirectoryNotFoundException();
            if (Directory.Exists(Path.Combine(FolderBrowseCommand.FolderPath, ProjectModelI.ProjectModel.name.value)))
                throw new Core.AlreadyExistsError();
            NewProject.ProjectManager.ProjectManagerViewModel.nextButtonPressed();
        }

        private void onPropertyRaised(string property_name) {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(property_name));
        }
    }
}
