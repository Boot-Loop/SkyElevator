using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Core.Data.Files;
using SkyElevator.src.view_models.commands;
using SkyElevator.src.views.project_manager_views.sub_views;
using CoreApp = Core.Application;

namespace SkyElevator.src.view_models
{
    public class RecentProjectModel
    {
        public string DisplayName { get; set; }
        public string Path { get; set; }

        public static List<RecentProjectModel> getRecentProjects() {
            List<ProgrameData.ProjectViewData> recent_projects_fetched = CoreApp.singleton.getRecentProjects();
            List<RecentProjectModel> recent_projects = new List<RecentProjectModel>();
            foreach (ProgrameData.ProjectViewData recent_project in recent_projects_fetched) {
                string display_name = string.Format("{0} ({1})", recent_project.name, recent_project.client_name);
                string path = recent_project.path;
                RecentProjectModel recent_project_model = new RecentProjectModel() { DisplayName = display_name, Path = path };
                recent_projects.Add(recent_project_model);
            }
            return recent_projects;
        }
    }

    public class OpenProjectViewModel
    {
        public RelayCommand ItemSelectedCommand { get; private set; }
        public RelayCommand OpenExistingProjectCommand { get; private set; }
        public List<RecentProjectModel> RecentProjects { get; private set; }
        public OpenProject OpenProject { get; set; }
        public string ExistingProjectPath { get; set; }
        public bool NoRecentProjectsVisibility { get; set; }
        public int SelectedIndex { get; set; }

        public OpenProjectViewModel(OpenProject open_project)
        {
            ItemSelectedCommand = new RelayCommand(itemDoublelClicked);
            OpenExistingProjectCommand = new RelayCommand(openExistingProject);
            RecentProjects = RecentProjectModel.getRecentProjects();
            OpenProject = open_project;
            if (RecentProjects.Count == 0) NoRecentProjectsVisibility = true;
            else NoRecentProjectsVisibility = false;
        }

        private void itemDoublelClicked(object sender) {
            CoreApp.singleton.loadProject(SelectedIndex);
            OpenProject.ProjectManager.closeWindow();
        }

        private void openExistingProject(object sender) {
            OpenFileDialog open_file_dialog = new OpenFileDialog();
            open_file_dialog.InitialDirectory = CoreApp.singleton.programe_data_file.data.default_proj_dir;
            open_file_dialog.DefaultExt = Core.Reference.PROJECT_FILE_EXTENSION;
            DialogResult result = open_file_dialog.ShowDialog();
            if (result == DialogResult.OK) ExistingProjectPath = open_file_dialog.FileName;
            CoreApp.singleton.addAndLoadExistingProject(ExistingProjectPath);
            OpenProject.ProjectManager.closeWindow();
        }
    }
}
