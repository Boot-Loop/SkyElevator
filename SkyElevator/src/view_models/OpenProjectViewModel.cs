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
    //public class RecentProjectModel
    //{
    //    public string DisplayName { get; set; }
    //    public string Path { get; set; }

    //    private List<ProgrameData.ProjectViewData> _recent_project_datas = new List<ProgrameData.ProjectViewData>();

    //    public List<ProgrameData.ProjectViewData> RecentProjectDatas
    //    {
    //        get { return _recent_project_datas; }
    //        set { _recent_project_datas = value; }
    //    }

    //    int index = 0;
    //    public string Name
    //    {
    //        get
    //        {
    //            var list = Core.Application.getSingleton().getRecentProjects();
    //            return (list.Count > 0) ? list[index++].name : "";
    //        }
    //    }

    //    public RecentProjectModel()
    //    {
    //        foreach (ProgrameData.ProjectViewData project_view_data in CoreApp.getSingleton().getRecentProjects()) {
    //            RecentProjectDatas.Add(project_view_data);
    //            DisplayName = string.Format("{0} ({1})", project_view_data.name, project_view_data.client_name);
    //            Path = project_view_data.path;
    //        }
    //    }
    //}

    //public class OpenProjectViewModel: INotifyPropertyChanged
    //{
    //    //public RelayCommand SelectCommand { get; set; }
    //    ////public RecentProjectModel RecentProjectModel { get; set; }
    //    //public bool NoRecentProjectsVisibility { get; set; }
    //    //public int SelectedIndex { get; set; }

    //    ////private List<ProgrameData.ProjectViewData> _recent_project_datas = new List<ProgrameData.ProjectViewData>();

    //    ////public List<ProgrameData.ProjectViewData> RecentProjectDatas
    //    ////{
    //    ////    get { return CoreApp.getSingleton().getRecentProjects(); }
    //    ////    //set { _recent_project_datas = value; }
    //    ////}

    //    //public List<RecentProjectModel> RecentProjectDatas = new List<RecentProjectModel>() { new RecentProjectModel() { DisplayName = "asd" }, new RecentProjectModel() { DisplayName = "asd" } };

    //    public RelayCommand ItemSelectedCommand { get; private set; }

    //    public int SelectedIndex { get; set; }

    //    public List<RecentProjectModel> Items { get; private set; }

    //    public OpenProjectViewModel() {
    //        //RecentProjectModel = new RecentProjectModel();
    //        //if (RecentProjectModel.RecentProjectDatas.Count == 0) NoRecentProjectsVisibility = true;
    //        //else NoRecentProjectsVisibility = false;
    //        ItemSelectedCommand = new RelayCommand(testFunction);
    //        Items = RecentProjectModel.GetItems();
    //    }

    //    public event PropertyChangedEventHandler PropertyChanged;

    //    private void testFunction(object parameter) {
    //        Console.WriteLine("Hello Azeem");
    //        Console.WriteLine(SelectedIndex);
    //    }

    //    private void onPropertyRaised(string prop)
    //    {
    //        if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(prop));
    //    }
    //}

    public class RecentProjectModel
    {
        public string DisplayName { get; set; }
        public string Path { get; set; }

        public static List<RecentProjectModel> getRecentProjects() {
            List<ProgrameData.ProjectViewData> recent_projects_fetched = CoreApp.getSingleton().getRecentProjects();
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
        public List<RecentProjectModel> RecentProjects { get; private set; }
        public bool NoRecentProjectsVisibility { get; set; }
        public int SelectedIndex { get; set; }

        public OpenProjectViewModel()
        {
            ItemSelectedCommand = new RelayCommand(OnItemSelected);
            RecentProjects = RecentProjectModel.getRecentProjects();
            if (RecentProjects.Count == 0) NoRecentProjectsVisibility = true;
            else NoRecentProjectsVisibility = false;
        }

        private void OnItemSelected(object sender)
        {
            Console.WriteLine("Hello" + " " + sender as string);
            Console.WriteLine(SelectedIndex);
        }
    }
}
