using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Core.Data.Files;
using SkyElevator.src.view_models.commands;
using CoreApp = Core.Application;

namespace SkyElevator.src.view_models
{
    public class RecentProjectModel {

        public string DisplayName { get; set; }
        public string Path { get; set; }

        public List<ProgrameData.ProjectViewData> RecentProjectDatas { 
            get { return Core.Application.getSingleton().getRecentProjects(); }
        }

        public RecentProjectModel()
        {
            foreach (ProgrameData.ProjectViewData project_view_data in CoreApp.getSingleton().getRecentProjects()) {
                RecentProjectDatas.Add(project_view_data);
                DisplayName = string.Format("{0} ({1})", project_view_data.name, project_view_data.client_name);
                Path = project_view_data.path;
            }
        }
    }

    public class OpenProjectViewModel
    {
        public RelayCommand SelectCommand { get; set; }
        public RecentProjectModel RecentProjectModel { get; set; }
        public int SelectedIndex { get; set; }
        public bool NoRecentProjectsVisibility { get; set; }

        public OpenProjectViewModel() {
            RecentProjectModel = new RecentProjectModel();
            if (RecentProjectModel.RecentProjectDatas.Count == 0) NoRecentProjectsVisibility = true;
            else NoRecentProjectsVisibility = false;
            SelectCommand = new RelayCommand(testFunction);
        }

        private void testFunction(object parameter) {
            Console.WriteLine("Hello Azeem");
            Console.WriteLine(SelectedIndex);
        }
    }
}
